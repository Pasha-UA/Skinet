using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Currency : BaseEntity
    {
        public string CurrencyId { get; set; }
        public string FullName { get; set; }
        public string CurrencyCode { get; set; }
        public string Symbol { get; set; }
        public decimal Rate { get; set; }
        public bool IsPrimary { get; set; }
    }
}