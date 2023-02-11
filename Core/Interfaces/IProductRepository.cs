using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(string id);
        Task<IReadOnlyList<Product>> GetProductsAsync();
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
        Task<ImportFile> SaveToDiskAsync(IFormFile file);
        void DeleteFromDisk(ImportFile importFile);
        public Task<bool> UpdatePriceListInDatabase(PriceListForImport file);

    }
}