using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersAsync()
        {
            var spec = new OrdersWithItemsAndOrderingSpecification();

            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }

        // public async Task<IReadOnlyList<AppUser>> GetUsersAsync()
        // {
        //     //  var spec = new UsersSpecification();

        //     //  return await _unitOfWork.Repository<AppUser>().ListAsync(spec);
        //     throw new NotImplementedException();
        // }

        public async Task<Order> UpdateOrderStatusAsync(string orderId, OrderStatus orderStatus)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(orderId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null) return null;

            order.Status = orderStatus;
            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.Complete();

            return order;

        }

        // public async Task<IReadOnlyList<OrderStatus>> GetOrderStatuses()
        // {

        //     // need logics
        //     return await Task.Run(() =>new List<OrderStatus>());
        // }

    }
}