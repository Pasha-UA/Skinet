using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ProductCategory : BaseEntity
    {
        public string ParentId { get; set; }
        public string Name { get; set; }

        public static bool operator ==(ProductCategory a, ProductCategory b)
        {
            if ((object)a == null || (object)b == null)
                return false;

            return (string.Compare(a.Name, b.Name) == 0
                && string.Compare(a.ParentId, b.ParentId) == 0);
        }
        
        public static bool operator !=(ProductCategory a, ProductCategory b)
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

            var other = (ProductCategory)obj;

            return (string.Compare(this.Name, other.Name) == 0 && string.Compare(this.ParentId, other.ParentId) == 0 && string.Compare(this.Id, other.Id) == 0);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}