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
                .Include(p => p.Prices)
                .FirstOrDefaultAsync(p => p.Id == id);

        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductCategory)
                .Include(p => p.Prices)
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
            if (File.Exists(Path.Combine("wwwroot/import", importFile.FileName)))
            {
                File.Delete("wwwroot/import" + importFile.FileName);
            }
        }

        public async Task<bool> UpdatePriceListInDatabase(PriceListForImport file)
        {

            return true;
        }

        public string GenerateRandomId(string chars, int length)
        {
            var random = new Random();
            string uniqueId = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return uniqueId;
        }

        /// generates random id using parameter length and chars 'abcdefghijklmnopqrstuvwxyz0123456789'
        public string GenerateRandomId(int length)
        {
            string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return GenerateRandomId(chars, length);
        }


    }
}