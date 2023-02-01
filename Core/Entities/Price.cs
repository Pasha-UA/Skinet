using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Price : BaseEntity
    {
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public Currency Currency { get; set; }
        public string CurrencyId { get; set; }

        public bool IsRetail { get; set; }
    }
}