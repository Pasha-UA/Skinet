using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.Comparers
{
    public class ComparerById<T> : IEqualityComparer<T> where T : BaseEntity
    {
        public bool Equals(T x, T y)
        {
            if (x == null || y == null) return false;

            return String.Compare(x.Id, y.Id) == 0;
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}