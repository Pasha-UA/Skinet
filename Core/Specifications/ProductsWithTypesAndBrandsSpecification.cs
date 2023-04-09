using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        // public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
        //     : base(x =>
        //          (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search) || x.BarCode.ToLower().Contains(productParams.Search)) &&
        //          (string.IsNullOrEmpty(productParams.BrandId) || x.ProductBrandId == productParams.BrandId) &&
        //          (string.IsNullOrEmpty(productParams.TypeId) || x.ProductTypeId == productParams.TypeId) &&
        //          (string.IsNullOrEmpty(productParams.CategoryId) || x.ProductCategoryId == productParams.CategoryId)
        //     )
        // {
        //     AddInclude(x => x.ProductType);
        //     AddInclude(x => x.ProductBrand);
        //     AddInclude(x => x.ProductCategory);
        //     AddInclude(x => x.Photos);
        //     // AddInclude(x => productParams.ShowBulkPrice ? x.Prices : x.Prices.Where(p => p.PriceType.IsBulk != true));
        //     if (productParams.ShowBulkPrice)
        //     {
        //         AddInclude(x => x.Prices);
        //     }
        //     else
        //     {
        //         AddInclude(x => x.Prices.Where(p => p.PriceType.IsBulk != true));
        //     }
        //     AddOrderBy(x => x.Name);
        //     ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

        //     if (!string.IsNullOrEmpty(productParams.Sort))
        //     {
        //         switch (productParams.Sort)
        //         {
        //             case "priceAsc":
        //                 AddOrderBy(p => p.Price);
        //                 break;
        //             case "priceDesc":
        //                 AddOrderByDescending(p => p.Price);
        //                 break;
        //             default:
        //                 AddOrderBy(n => n.Name);
        //                 break;
        //         }
        //     }
        // }

        public ProductsWithTypesAndBrandsSpecification(string id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.ProductCategory);
            AddInclude(x => x.Photos);
            AddInclude(x => x.Prices);

        }


        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
            : base(x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search))
                && (string.IsNullOrEmpty(productParams.BrandId) || x.ProductBrandId == productParams.BrandId)
                && (string.IsNullOrEmpty(productParams.TypeId) || x.ProductTypeId == productParams.TypeId)
                && (string.IsNullOrEmpty(productParams.CategoryId) || x.ProductCategoryId == productParams.CategoryId)
                && (productParams.Subcategories == null || productParams.Subcategories.Contains(x.ProductCategoryId))
                && (!productParams.VisibleOnly || x.Visible)
                )
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.Photos);

            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
        }
    }
}
