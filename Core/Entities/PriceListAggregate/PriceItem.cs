using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.PriceListAggregate
{
    public class PriceItem : PriceType
    {

        // public PriceType PriceType {get; set;}
        public decimal Price { get; set; }
        // public string ProductId { get; set; }

        public static bool operator ==(PriceItem a, PriceItem b)
        {
            if ((object)a == null || (object)b == null)
                return false;

            return (
                a.Quantity == b.Quantity
                && Decimal.Compare(a.Price, b.Price) == 0
                && string.Compare(a.CurrencyId, b.CurrencyId) == 0
                && string.Compare(a.Id, b.Id) == 0
                );
        }

        public static bool operator !=(PriceItem a, PriceItem b)
        {
            if ((object)a == null || (object)b == null)
                return false;

            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            var other = (PriceItem)obj;

            return (
                string.Compare(this.Id, other.Id) == 0
                && string.Compare(this.CurrencyId, other.CurrencyId) == 0
                && Decimal.Compare(this.Price, other.Price) ==  0
                && this.Quantity == other.Quantity
            );
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + this.Id.GetHashCode();
                hash = hash * 23 + this.Quantity.GetHashCode();
                hash = hash * 23 + this.CurrencyId.GetHashCode();
                // hash = hash * 23 + this.IsRetail.GetHashCode();
                hash = hash * 23 + this.Price.GetHashCode();
                return hash;
            }
        }
    }
}