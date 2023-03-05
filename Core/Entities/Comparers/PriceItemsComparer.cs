using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.PriceListAggregate;

namespace Core.Entities.Comparers
{
    public class PriceItemsComparer : IEqualityComparer<PriceItem>
    {

        public bool Equals(PriceItem x, PriceItem y)
        {
            if (x == y)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id && x.Quantity == y.Quantity && x.CurrencyId == y.CurrencyId && x.IsRetail == y.IsRetail && x.Price == y.Price;
        }

        public int GetHashCode(PriceItem obj)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + obj.Id.GetHashCode();
                hash = hash * 23 + obj.Quantity.GetHashCode();
                hash = hash * 23 + obj.CurrencyId.GetHashCode();
                hash = hash * 23 + obj.IsRetail.GetHashCode();
                hash = hash * 23 + obj.Price.GetHashCode();
                return hash;
            }
        }
    }
}
