using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.PriceListAggregate
{
    public class PriceType : BaseEntity
    {
        public int? Quantity { get; set; }

        public string CurrencyId { get; set; }

        public bool IsRetail { get; set; } = false;
        public bool IsBulk { get; set; } = false;
    }
}