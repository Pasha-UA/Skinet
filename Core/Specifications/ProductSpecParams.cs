using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string BrandId { get; set; }
        public string TypeId { get; set; }
        public string CategoryId { get; set; }
        public bool IncludeSubCategories { get; set; } = false;
        public string[] Subcategories { get; set; } = null;
        public string Sort { get; set; }
        public bool ShowBulkPrice { get; set; } = false;
        private string _search;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}