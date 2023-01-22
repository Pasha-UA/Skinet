using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Price : BaseEntity
    {
        public int MinOrderQuantity {get; set;}

        public decimal PriceForOne {get; set;} // come up with a suitable name

//        public decimal RoundTo {get; set;}
        public Currency Currency {get; set;} 
    }
}