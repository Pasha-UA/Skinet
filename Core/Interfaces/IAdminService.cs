using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces
{
    public interface IAdminService
    {
        Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus orderStatus);
        Task<Order> GetOrderByIdAsync(int id);
        Task<IReadOnlyList<Order>> GetOrdersAsync();
        // Task<IReadOnlyList<OrderStatus>> GetOrderStatuses(); // for Admin panel


    }
}