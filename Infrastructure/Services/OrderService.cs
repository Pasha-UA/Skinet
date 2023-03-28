using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string deliveryMethodId, string basketId, Address shippingAddress)
        {
            // get basket from the repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            //get items from the product repo
            var items = new List<OrderItem>();
            var nextOrderItemId = await SetNextId<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.Photos.FirstOrDefault(x => x.IsMain)?.PictureUrl);

                var productPrice = 0m;
                if (productItem.Prices !=null && productItem.Prices.Count>0)
                {
                    // вычисление цены товара в зависимости от заказанного количества
                    var filteredPrices = productItem.Prices.Where(x=>x.PriceType.Quantity <= item.quantity);
                    var maxQuantity = filteredPrices.Max(x=>x.PriceType.Quantity);
                    productPrice = filteredPrices.FirstOrDefault(x=>x.PriceType.Quantity==maxQuantity).Value;
                }
                else
                // временно, пока не избавился от поля Product.Price 
                {
                    productPrice = productItem.Price;
                }
                // var orderItem = new OrderItem(itemOrdered, productItem.Price, item.quantity);
                var orderItem = new OrderItem(itemOrdered, productPrice, item.quantity);
                orderItem.Id = nextOrderItemId;
                nextOrderItemId = (Convert.ToInt32(nextOrderItemId) + 1).ToString(); // not safe...
                items.Add(orderItem);
            }

            // get delivery method from repo 
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            // calc subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);


            //create order
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);
            // get order status from repo
            order.Status = await _unitOfWork.Repository<OrderStatus>().GetByIdAsync(order.StatusId);
            order.Id = await SetNextId<Order>();
            _unitOfWork.Repository<Order>().Add(order);

            //save to db
            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            // delete basket

            await _basketRepo.DeleteBasketAsync(basketId);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<DeliveryMethod> AddDeliveryMethodAsync(DeliveryMethod deliveryMethod)
        {
            deliveryMethod.Id = await SetNextId<DeliveryMethod>();
            _unitOfWork.Repository<DeliveryMethod>().Add(deliveryMethod);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return null;
            return deliveryMethod;
        }

        public async Task<Order> GetOrderByIdAsync(string id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);

            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }

        public async Task<IReadOnlyList<OrderStatus>> GetOrderStatusList()
        {
            return await _unitOfWork.Repository<OrderStatus>().ListAllAsync();
            //            return new OrderStatusList();
        }

        private async Task<string> SetNextId<T>() where T : BaseEntity
        {
            var entities = await _unitOfWork.Repository<T>().ListAllAsync();
            int lastId = 0;
            if (entities.Any())
            {
                lastId = entities.Max(o => Convert.ToInt32(o.Id));
            }
            return (lastId + 1).ToString();
        }
    }
}