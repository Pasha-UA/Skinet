using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Comparers;

namespace Core.Entities.PriceListAggregate
{
    public class Price : BaseEntity
    {

        public decimal Value { get; set; }
        public string ProductId { get; set; }
        // public Product Product { get; set; }
        public string PriceTypeId { get; set; }
        public PriceType PriceType {get; set;}

        public DateTimeOffset DateTime { get; set; } = DateTimeOffset.Now;

    }
}