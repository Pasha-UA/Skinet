using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class WholesalePrice
    {
        public int MinOrderQuantity {get; set;}

        public decimal Price {get; set;}

        public string Currency {get; set;}
    }
}