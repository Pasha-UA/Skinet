using System.ComponentModel.DataAnnotations;
using Core.Entities.PriceListAggregate;

namespace API.Dtos
{
    public class ProductCreateDto
    {
        public string Id { get; set; }
        public string ExternalId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [RegularExpression(@"^\$?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$",
            ErrorMessage = "Price must be a decimal (e.g 20.30)")]
        public decimal Price { get; set; }

        public string PictureUrl { get; set; }

        [Required]
        public string ProductTypeId { get; set; }

        [Required]
        public string ProductBrandId { get; set; }

        [Required]
        public string ProductCategoryId { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public string BarCode { get; set; }
        public List<Price> Prices { get; set; }
        public bool Visible { get; set; }
        public bool Deleted { get; set; }

        // public PriceItem BulkPrice { get; set; }
    }
}