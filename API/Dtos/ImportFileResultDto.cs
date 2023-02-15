using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ImportFileResultDto
    {
        public bool Success { get; set; } = true;
        public int ProductsTotal { get; set; } = 0;
        public int ProductsNotUpdated { get; set; } = 0; // not changed
        public int ProductsUpdateSuccessCount { get; set; } = 0;
        public int ProductsUpdateErrorsCount { get; set; } = 0;
        public int ProductsCreateErrorsCount { get; set; } = 0;
        public int ProductsCreated { get; set; } = 0;

        public int ProductsNotFound { get; set; } = 0;

        public int CategoriesCreated { get; set; } = 0;
        public int CategoriesNotUpdated { get; set; } = 0; // not changed
        public int CategoriesUpdateSuccessCount { get; set; } = 0;
        public int CategoriesCreateErrorsCount { get; set; } = 0;
        public int CategoriesUpdateErrorsCount { get; set; } = 0;

    }
}