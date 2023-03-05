using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.PriceListAggregate;

namespace API.Dtos
{
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