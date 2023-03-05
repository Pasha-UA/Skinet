using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.PriceListAggregate
{
    public class ImportFileResult
    {
        public int ProductsTotal { get; set; } = 0;
        public int ProductsNotUpdated { get; set; } = 0; // not changed
        public int ProductsUpdateSuccess { get; set; } = 0;
        public int ProductsUpdateErrors { get; set; } = 0;
        public int ProductsCreateErrors { get; set; } = 0;
        public int ProductsCreated { get; set; } = 0;
        public int ProductsNotFound { get; set; } = 0;
        public int CategoriesCreated { get; set; } = 0;
        public int CategoriesNotUpdated { get; set; } = 0; // not changed
        public int CategoriesUpdateSuccess { get; set; } = 0;
        public int CategoriesCreateErrors { get; set; } = 0;
        public int CategoriesUpdateErrors { get; set; } = 0;
    }
    
}