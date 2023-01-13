using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Infrastructure.Identity.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;
        private readonly IMapper _mapper;

        private readonly IOrderService _orderService;

        public AdminController(ILogger<AdminController> logger, IMapper mapper, IAdminService adminService, IOrderService orderService)
        {
            _orderService = orderService;
            _adminService = adminService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("orders")]
//        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders()
        {
            var orders = await _adminService.GetOrdersAsync();
            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }


        [HttpPut("order/{id}")]
        // [Authorize]
//                [Authorize(Roles = "Admin")]
//[Authorize(Policy ="AdminRoleClaim")]
//[Authorize(Roles ="Admin")]
//[Authorization(Roles: "Admin")]

//        [Authorize(Policy ="AdminOnly")]
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


        [HttpGet("orderstatuses")]
        public ActionResult<IReadOnlyList<OrderStatus>> GetOrderStatuses()
        {
            var statuses = _orderService.GetOrderStatuses().Statuses;

            return Ok(statuses);
        }


    }
}