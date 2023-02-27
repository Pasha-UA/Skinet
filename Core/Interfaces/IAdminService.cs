using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces
{
    public interface IAdminService
    {
        Task<Order> UpdateOrderStatusAsync(string orderId, OrderStatus orderStatus);
        Task<Order> GetOrderByIdAsync(string id);
        Task<IReadOnlyList<Order>> GetOrdersAsync();
        // Task<IReadOnlyList<OrderStatus>> GetOrderStatusList(); // for Admin panel

    }
}