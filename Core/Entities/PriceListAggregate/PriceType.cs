using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.PriceListAggregate
{
    public class PriceType : BaseEntity
    {
        public int Quantity { get; set; } = 1;
        public string CurrencyId { get; set; } = "UAH";
        public Currency Currency { get; set; }
        public bool IsRetail { get; set; } = false;
        public bool IsBulk { get; set; } = false;
    }
}