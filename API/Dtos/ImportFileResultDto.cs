using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ImportFileResult
    {
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
    
    public class ImportFileResultDto
    {
        public ImportFileResultDto(bool success, ImportFileResult result)
        {
            Success = success;
            Data = result;
        }

        public bool Success { get; set; }
        public ImportFileResult Data { get; set; }
    }
}