using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
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
            // var spec = new OrdersWithItemsAndOrderingSpecification(orderId);
            // var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            var spec = new OrdersWithItemsAndOrderingSpecification();

            var orders = await _unitOfWork.Repository<Order>().ListAsync(spec);

            var order = orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null) return null;

            order.Status = orderStatus;
            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.Complete();

            return order;

        }
        // private async Task<string> SetNextId()
        // {
        //     var roles = await _unitOfWork.Repository<T>().ListAllAsync();
        //     int lastId = 0;
        //     if (entities.Any())
        //     {
        //         lastId = entities.Max(o => Convert.ToInt32(o.Id));
        //     }
        //     return (lastId + 1).ToString();
        // }

        // public async Task<AppRole> CreateRole (AppRole role)
        // {
        //     role.Id = this.SetNextId<AppRole>();
        // }

        // public async Task<IReadOnlyList<OrderStatus>> GetOrderStatusList()
        // {

        //     // need logics
        //     return await Task.Run(() =>new List<OrderStatus>());
        // }

    }
}