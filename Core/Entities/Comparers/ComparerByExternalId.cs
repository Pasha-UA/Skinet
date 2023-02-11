using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.Comparers
{
    public class ComparerByExternalId<T> : IEqualityComparer<T> where T : Product
    {
        public bool Equals(T x, T y)
        {
            if (x == null || y == null) return false;

            return String.Compare(x.ExternalId, y.ExternalId) == 0;
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return obj.ExternalId.GetHashCode();
        }
    }
}