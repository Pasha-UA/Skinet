using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;
        private readonly IMapper _mapper;

        private readonly IOrderService _orderService;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(
            ILogger<AdminController> logger,
            IMapper mapper,
            IAdminService adminService,
            IOrderService orderService,
            IUserRepository userRepository,
            UserManager<AppUser> userManager
            )
        {
            _orderService = orderService;
            _userRepository = userRepository;
            _userManager = userManager;
            _adminService = adminService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("orders")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders()
        {
            var orders = await _adminService.GetOrdersAsync();
            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpPut("order/{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<Order>> UpdateOrderStatus(int id, [FromQuery] string orderStatusId)
        {
            var orderStatus = (OrderStatus)_orderService.GetOrderStatuses().Statuses.First(s => s.Id == int.Parse(orderStatusId)).Id;
            var order = await _adminService.UpdateOrderStatusAsync(id, orderStatus);

            if (order == null)
            {
                _logger.LogError("Error updating order status");
                return (BadRequest("Не удалось обновить статус заказа"));
            }

            _logger.LogInformation("Order {0} status updated {1}", id, orderStatus);

            return Ok(order);
        }

        // get list of users
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IReadOnlyList<UserDto>>> GetUsersAsync()
        {
            var usersWithRoles = new List<UserDto>();
            var roles = _userRepository.GetRolesAsync();
            var users = _userRepository.GetUsersAsync();
            var userRoles = _userRepository.GetUserRolesAsync();

            var userRolesJoined = userRoles
                .Join(roles, ur => ur.RoleId, r => r.Id, (userRoles, roles) => new { RoleId = userRoles.RoleId, UserId = userRoles.UserId, RoleName = roles.Name })
                .Join(users, r => r.UserId, u => u.Id, (r, u) => new { r.RoleId, r.RoleName, u.Id, u.Email, u.Address, u.UserName, u.DisplayName, u.PhoneNumber });

            var userRolesJoinedAndGroupped = userRolesJoined
                .GroupBy(u => u.Email)
                .Select(g => g.ToList())
            ;

            foreach (var grouppedUser in userRolesJoinedAndGroupped)
            {
                var grouppedUserEmail = grouppedUser.Select(a => a.Email).First();
                var user = await _userManager.FindByEmailAsync(grouppedUserEmail);
                var grouppedUserRoles = grouppedUser.Select(a => new AppRole { Id = a.RoleId, Name = a.RoleName }).ToList();
                var grouppedUserDisplayName = grouppedUser.Select(a => a.DisplayName).First();
                var emailConfirmationRequired = _userRepository.EmailConfirmationRequired(user);
                var userDto = new UserDto
                {
                    Email = grouppedUserEmail,
                    Roles = grouppedUserRoles,
                    DisplayName = grouppedUserDisplayName,
                    PhoneNumber = user.PhoneNumber,
                    EmailConfirmationRequired = emailConfirmationRequired,
                    AccountLocked = (Nullable.Compare(user.LockoutEnd, DateTimeOffset.Now) < 0 && user.LockoutEnabled && user.LockoutEnd != null)
                };
                usersWithRoles.Add(userDto);
            }
            return Ok(usersWithRoles);
        }

        [HttpGet("usersforrole")]
        [Authorize]
        public ActionResult<IEnumerable<AppUser>> GetUsersForRole([FromQuery] string roleName)
        {
//            List<UserDto>
            List<AppUser> users = new List<AppUser>();
            var role = _userRepository.GetRolesAsync().SingleOrDefault(r => r.Name.ToLower() == roleName.ToLower(), null);
            if (role != null)
            {
                users.AddRange(_userRepository.GetUsersForRole(role));
            }
            return users;
        }

        [HttpGet("orderstatuses")]
        public ActionResult<IReadOnlyList<OrderStatus>> GetOrderStatuses()
        {
            var statuses = _orderService.GetOrderStatuses().Statuses;

            return Ok(statuses);
        }


    }
}