using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, string deliveryMethodId, string basketId, Address shippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync (string buyerEmail);
        Task<Order> GetOrderByIdAsync(string id, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
        Task<IReadOnlyList<OrderStatus>> GetOrderStatusList();
        Task<DeliveryMethod> AddDeliveryMethodAsync(DeliveryMethod deliveryMethod);
    }
}