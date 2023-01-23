using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ImportFileResultDto
    {
        public bool Success { get; set; } = false;
        public int ProductsTotal { get; set; } = 0;
        public int ProductsUpdateSuccessCount { get; set; } = 0;
        public int ProductsUpdateErrorsCount { get; set; } = 0;
        public int ProductsCreated { get; set; } = 0;
    }
}