using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecParams productParams)
        : base(x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search) || x.BarCode.ToLower().Contains(productParams.Search)) 
                && (string.IsNullOrEmpty(productParams.BrandId) || x.ProductBrandId == productParams.BrandId) 
                && (string.IsNullOrEmpty(productParams.TypeId) || x.ProductTypeId == productParams.TypeId) 
                && (string.IsNullOrEmpty(productParams.CategoryId) || x.ProductCategoryId == productParams.CategoryId) 
                && (productParams.Subcategories == null || productParams.Subcategories.Contains(x.ProductCategoryId))
                && (!productParams.VisibleOnly || x.Visible)
                
            )
        {

        }
    }
}