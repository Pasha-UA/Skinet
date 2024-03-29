using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrdersWithItemsAndOrderingSpecification : BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpecification() : base()
        {
            //AddInclude(o => o.Id);
            // AddInclude(o => o.BuyerEmail);
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            //AddInclude(o => o.Status);
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrdersWithItemsAndOrderingSpecification(string email) : base(o => o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            //AddInclude(o => o.Status);
            AddOrderByDescending(o => o.OrderDate);
        }
        // public OrdersWithItemsAndOrderingSpecification(string id) : base(o => o.Id == id)
        // {
        //     AddInclude(o => o.OrderItems);
        //     AddInclude(o => o.DeliveryMethod);
        //     //AddInclude(o => o.Status);
        // }

        public OrdersWithItemsAndOrderingSpecification(string id, string email) : base(o => o.Id == id && o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            //AddInclude(o => o.Status);
        }
    }
}