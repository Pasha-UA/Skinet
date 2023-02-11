using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(p => p.Id == id);

        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductCategory)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }

        public async Task<ImportFile> SaveToDiskAsync(IFormFile file)
        {
            var importFile = new ImportFile();
            if (file.Length > 0)
            {
                var fileName = DateTimeOffset.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine("wwwroot/import", fileName);
                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(fileStream);

                importFile.FileName = filePath;
                importFile.ImportFileUrl = "import/" + fileName;

                return importFile;
            }

            return null;
        }
 
        public void DeleteFromDisk(ImportFile importFile)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePriceListInDatabase(PriceListForImport file)
        {
            
            return true;
        }

   }
}