using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ProductToReturnDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string ProductType { get; set; }
        public string ProductBrand { get; set; }
        public string ProductCategory { get; set; }
        public string BarCode { get; set; }
        public string Stock { get; set; }
        public IEnumerable<PhotoToReturnDto> Photos { get; set; }
    }
}