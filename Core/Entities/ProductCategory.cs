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
    }
}