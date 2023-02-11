using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Xml.Serialization;
using Infrastructure.Data.Config;
using System.Data;
using System.Xml;
using Core.Entities.Comparers;
using Core.Entities.PriceListAggregate;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public IMapper _mapper { get; }
        private readonly IPhotoService _photoService;
        private readonly IProductRepository _productRepository;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _photoService = photoService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _unitOfWork.Repository<Product>().CountAsync(countSpec);

            var products = await _unitOfWork.Repository<Product>().ListAsync(spec);

            var data = _mapper
                .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(string id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        // [Cached(600)]
        // [HttpGet()]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        // public async Task<ActionResult<ProductToReturnDto>> GetProductByBarCode([FromQuery]string barCode)
        // {
        //     var spec = new ProductsWithTypesAndBrandsSpecification(barCode);

        //     var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

        //     if (product == null) return NotFound(new ApiResponse(404));

        //     return _mapper.Map<Product, ProductToReturnDto>(product);
        // }

        [Cached(1000)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _unitOfWork.Repository<ProductBrand>().ListAllAsync());
        }

        [Cached(1000)]
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetProductCategories()
        {
            return Ok(await _unitOfWork.Repository<ProductCategory>().ListAllAsync());
        }

        // [Cached(1000)]
        // [HttpGet("categoriesTree")]
        // public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetProductCategoriesTree()
        // {
        //     var categories = await _unitOfWork.Repository<ProductCategory>().ListAllAsync();

        //     // TODO: make return as tree ?
        //     return Ok(categories);
        // }

        [Cached(1000)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _unitOfWork.Repository<ProductType>().ListAllAsync());
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<Product>> CreateProduct(ProductCreateDto productToCreate)
        {
            var product = _mapper.Map<ProductCreateDto, Product>(productToCreate);

            _unitOfWork.Repository<Product>().Add(product);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating product"));

            return Ok(product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<Product>> UpdateProduct(string id, ProductCreateDto productToUpdate)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

            _mapper.Map(productToUpdate, product);

            _unitOfWork.Repository<Product>().Update(product);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating product"));

            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

            foreach (var photo in product.Photos)
            {
                // if (photo.Id > 18)
                // {
                _photoService.DeleteFromDisk(photo);
                // }
            }

            _unitOfWork.Repository<Product>().Delete(product);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting product photo"));

            return Ok();
        }

        [HttpPut("{id}/photo")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductToReturnDto>> AddProductPhoto(string id, [FromForm] ProductPhotoDto photoDto)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            if (photoDto.Photo.Length > 0)
            {
                var photo = await _photoService.SaveToDiskAsync(photoDto.Photo);

                if (photo != null)
                {
                    product.AddPhoto(photo.PictureUrl, photo.FileName);

                    _unitOfWork.Repository<Product>().Update(product);

                    var result = await _unitOfWork.Complete();

                    if (result <= 0) return BadRequest(new ApiResponse(400, "Problem adding photo product"));
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "problem saving photo to disk"));
                }
            }

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }


        [HttpPost("import")]
        //   [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<ImportFileResultDto>> ImportProducts([FromForm] IFormFile importFile)
        {
            var result = new ImportFileResultDto();
            // read file

            try
            {
                long size = importFile.Length;

                var file = await _productRepository.SaveToDiskAsync(importFile);

                if (file == null) return new ImportFileResultDto { Success = false };

                var priceList = new PriceListForImport();

                if (await priceList.Import(file) == true)
                {
                    var categoriesInDb = await _unitOfWork.Repository<ProductCategory>().ListAllAsync();

                    foreach (var category in priceList.Categories)
                    {
                        if (categoriesInDb == null || !categoriesInDb.Contains(category, new ComparerById<ProductCategory>()))
                        {
                            // create new category and save it to DB
                            _unitOfWork.Repository<ProductCategory>().Add(category);

                            var res = await _unitOfWork.Complete();

                            if (res <= 0) result.CategoriesCreateErrorsCount++;

                            result.CategoriesCreated++;
                        }
                        else if (categoriesInDb.Contains(category, new ComparerById<ProductCategory>()))
                        {
                            var categoryInDb = categoriesInDb.First(c => c.Id == category.Id);
                            if (category == categoryInDb)
                            {
                                // don't update, category not changed
                                result.CategoriesNotUpdated++;
                            }
                            else
                            {
                                // category changed, update
                                _unitOfWork.Repository<ProductCategory>().Update(category);

                                var res = await _unitOfWork.Complete();

                                if (res <= 0) result.CategoriesUpdateErrorsCount++;

                                result.CategoriesUpdateSuccessCount++;
                            }
                        }
                    }

                    // import changed products to db
                    //                await _productRepository.UpdatePriceListInDatabase(priceList);

                    var productsInDb = await _unitOfWork.Repository<Product>().ListAllAsync();
                    foreach (var offer in priceList.Offers)
                    {
                        //                    var productCreate = _mapper.Map<OfferItem, ProductCreateDto>(offer);
                        var product = _mapper.Map<OfferItem, Product>(offer);
                        if (productsInDb == null || !productsInDb.Contains(product, new ComparerByExternalId<Product>()))
                        {
                            // create new product and save it to DB
                            product.Id = DateTime.UtcNow.Ticks.ToString();
                            if (string.IsNullOrEmpty(product.Description)) product.Description = "";
                            _unitOfWork.Repository<Product>().Add(product);

                            var res = await _unitOfWork.Complete();

                            if (res <= 0) result.ProductsCreateErrorsCount++;

                            result.ProductsCreated++;
                        }
                        else if (productsInDb.Contains(product, new ComparerByExternalId<Product>()))
                        {
                            var productInDb = productsInDb.First(c => c.ExternalId == product.ExternalId);
                            if (product == productInDb)
                            {
                                // don't update, product not changed
                                result.ProductsNotUpdated++;
                            }
                            else
                            {
                                // product changed, update
                                _unitOfWork.Repository<Product>().Update(product);

                                var res = await _unitOfWork.Complete();

                                if (res <= 0) result.ProductsUpdateErrorsCount++;

                                result.ProductsUpdateSuccessCount++;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Import file error: {0}", e);
                result.Success = false;
            }
            return result;
        }


        [HttpDelete("{id}/photo/{photoId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProductPhoto(string id, string photoId)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            var photo = product.Photos.SingleOrDefault(x => x.Id == photoId);

            if (photo != null)
            {
                if (photo.IsMain)
                    return BadRequest(new ApiResponse(400,
                        "You cannot delete the main photo"));

                _photoService.DeleteFromDisk(photo);
            }
            else
            {
                return BadRequest(new ApiResponse(400, "Photo does not exist"));
            }

            product.RemovePhoto(photoId);

            _unitOfWork.Repository<Product>().Update(product);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem adding photo product"));

            return Ok();
        }

        [HttpPost("{id}/photo/{photoId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductToReturnDto>> SetMainPhoto(string id, string photoId)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            if (product.Photos.All(x => x.Id != photoId)) return NotFound();

            product.SetMainPhoto(photoId);

            _unitOfWork.Repository<Product>().Update(product);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem adding photo product"));

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }
    }
}