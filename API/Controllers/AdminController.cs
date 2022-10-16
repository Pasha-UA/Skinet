using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    
    public class AdminController : BaseApiController
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;
        private readonly IMapper _mapper;


        public AdminController(ILogger<AdminController> logger, IMapper mapper,IAdminService adminService )
        {
            _adminService = adminService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("orders")] 
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders()
        {
            var orders = await _adminService.GetOrdersAsync();
            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>> (orders));
        }


        [HttpPut("order/{id}")]
        public async Task<ActionResult<Order>> UpdateOrderStatus(int orderId, OrderStatus orderStatus)
        {
            var order = await _adminService.UpdateOrderStatusAsync(orderId, orderStatus);
            _logger.LogInformation("Order status updated", orderStatus);

            return order;
        }



        // [HttpGet("orderstatuses")] 
        // public async Task<ActionResult<IReadOnlyList<OrderStatus>>> GetOrderStatuses()
        // {
        //     var statuses = await _orderService.GetOrderStatuses();

        //     return Ok((IReadOnlyList<OrderStatus>)(statuses));
        // }


    }
}