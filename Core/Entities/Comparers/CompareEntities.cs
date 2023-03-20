using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Core.Entities.Comparers
{
    public class CompareEntities<T> : IEqualityComparer<T> where T : BaseEntity
    {
        private PropertyInfo[] _properties { get; set; }
        public CompareEntities()
        {
            _properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.PropertyType.IsPrimitive 
                            || p.PropertyType == typeof(string) 
                            || p.PropertyType == typeof(decimal) 
                            || p.PropertyType == typeof(int)
                            || p.PropertyType == typeof(int?))
                .ToArray();
        }

        public bool Equals(T x, T y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;

            foreach (PropertyInfo property in _properties)
            {
                if (property.Name == "Id") continue;

                var valueX = property.GetValue(x);
                var valueY = property.GetValue(y);

                if (valueX == null)
                {
                    if (valueY != null)
                    {
                        return false;
                    }
                }

                if (!object.Equals(valueX, valueY))
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(T obj)
        {
            if (obj == null) return 0;

            int hash = 17;
            foreach (PropertyInfo property in _properties)
            {
                if (property.Name == "Id") continue;

                var value = property.GetValue(obj);
                if (value != null)
                {
                    hash = hash * 23 + value.GetHashCode();
                }
            }
            return hash;
        }
    }

}

