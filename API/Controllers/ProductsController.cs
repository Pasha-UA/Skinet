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
using System.Security.Cryptography;
using System.Reflection;
using System.Text;

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
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            if (productParams.CategoryId != null)
            {
                if (productParams.IncludeSubCategories)
                {
                    var subcategories = await _unitOfWork.Repository<ProductCategory>().GetAllChildrenCategoriesAsync(productParams.CategoryId);
                    if (subcategories == null)
                    {
                        return NotFound(new ApiResponse(404, "Category not found"));
                    }
                    productParams.CategoryId = null;
                    productParams.Subcategories = subcategories.Select(c => c.Id).ToArray();
                }
            }

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


        [Cached(1000)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _unitOfWork.Repository<ProductBrand>().ListAllAsync());
        }

        [Cached(1000)]
        [HttpGet("pricetypes")]
        public async Task<ActionResult<IReadOnlyList<PriceType>>> GetProductPriceTypes()
        {
            return Ok(await _unitOfWork.Repository<PriceType>().ListAllAsync());
        }

        [Cached(1000)]
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetProductCategories()
        {
            return Ok(await _unitOfWork.Repository<ProductCategory>().ListAllAsync());
        }

        [Cached(1000)]
        [HttpGet("subcategories/{id?}")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetAllSubcategories(string id)
        {
            var allCategories = await _unitOfWork.Repository<ProductCategory>().GetAllChildrenCategoriesAsync(id);

            if (allCategories == null)
            {
                return NotFound(new ApiResponse(404, new String($"Category with Id='{id}' not found.")));
            }

            var subcategories = allCategories.Select(c => c.Id).ToList();

            return Ok(subcategories);
        }


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
            var result = await this.ProductCreate(productToCreate);

            if (result.Item1 <= 0) return BadRequest(new ApiResponse(400, "Problem creating product"));

            var createdProduct = result.Item2;

            return Ok(createdProduct);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<Product>> UpdateProduct(string id, ProductCreateDto productToUpdate)
        {
            var result = await this.ProductUpdate(id, productToUpdate);

            if (result.Item1 <= 0) return BadRequest(new ApiResponse(400, "Problem updating product"));

            var updatedProduct = result.Item2;

            return Ok(updatedProduct);
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
            // TODO: add logics for parameters, parameters should come together with import file
            var importParameters = new ImportFileParameters();

            var result = await this.ImportPriceListFromFile(importFile, importParameters);
            if (result == null) return BadRequest(new ApiResponse(400, "Error on file import"));
            return new ImportFileResultDto(success: true, result: result);
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

        private bool EqualProductWithProductCreate(Product product, ProductCreateDto productCreateDto, ImportFileParameters parameters)
        {
            var pricesNotChanged = true;
            if (product.Prices != null && productCreateDto.Prices != null)
            {
                var comparerPrice = new CompareEntities<Price>();
                pricesNotChanged = product.Prices.OrderBy(p => p.PriceTypeId).SequenceEqual(productCreateDto.Prices.OrderBy(p => p.PriceTypeId), comparerPrice);
            }
            else if (product.Prices == null && productCreateDto.Prices == null)
            {
                // Both sequences are null, so the prices have not changed
                pricesNotChanged = true;
            }
            else
            {
                // One sequence is null, so the prices have changed
                pricesNotChanged = false;
            }

            var comparer = new CompareEntities<Product>();
            var productNotChanged = comparer.Equals(product, _mapper.Map<ProductCreateDto, Product>(productCreateDto));

            return productNotChanged
                    && pricesNotChanged;
        }

        private async Task<ImportFileResult> ImportPriceListFromFile(IFormFile importFile, ImportFileParameters parameters)
        {
            var result = new ImportFileResult();

            try
            {
                long size = importFile.Length;

                // read file
                var file = await _productRepository.SaveToDiskAsync(importFile);

                if (file == null)
                    return null;

                _productRepository.DeleteFromDisk(file);

                var priceList = new PriceListForImport();

                if (await priceList.Import(file) == true)
                {
                    result.ProductsTotal = priceList.Offers.Count();

                    _unitOfWork.Repository<Currency>().UpdateList(priceList.Currencies);
                    _unitOfWork.Repository<PriceType>().UpdateList(priceList.PriceTypes);
                    _unitOfWork.Repository<ProductCategory>().UpdateList(priceList.Categories);
                    await _unitOfWork.Complete();

                    // import changed products to db
                    var productsInDb = await _unitOfWork.Repository<Product>().ListAllAsync();

                    // find a list of products presenting in DB but not presenting in import file
                    var notFoundProducts = new List<Product>(productsInDb.Where(p => priceList.Offers.All(offer => offer.Id != p.ExternalId)));

                    result.ProductsNotFound = notFoundProducts.Count;

                    if (parameters.NotFoundProduct != NotFoundProduct.Ignore)
                    {
                        foreach (var product in notFoundProducts)
                        {
                            switch (parameters.NotFoundProduct)
                            {
                                case NotFoundProduct.Hide:
                                    product.Visible = false;
                                    break;

                                case NotFoundProduct.Delete: // safe delete
                                    product.DeletedOn = DateTime.Now;
                                    break;

                                case NotFoundProduct.NoStock:
                                    product.Stock = 0;
                                    product.Visible = true;
                                    break;

                                default:
                                    break;
                            }

                            var modifiedProduct = _mapper.Map<Product, ProductCreateDto>(product);

                            var res = await this.ProductUpdate(product.Id, modifiedProduct);

                            if (res.Item1 > 0)
                            {
                                result.ProductsUpdateSuccess++;
                            }

                            else result.ProductsUpdateErrors++;

                        }
                    }

                    foreach (var offer in priceList.Offers)
                    {
                        var productCreate = _mapper.Map<OfferItem, ProductCreateDto>(offer);

                        Product productInDb = productsInDb.FirstOrDefault(x => x.ExternalId == productCreate.ExternalId) ?? null;

                        if (productInDb is null) // no such product in db
                        {
                            // create new product and save it to DB
                            var res = await this.ProductCreate(productCreate);

                            if (res.Item1 > 0)
                            {
                                result.ProductsCreated++;
                            }
                            else result.ProductsCreateErrors++;
                        }
                        else // product exists
                        {
                            productCreate.Id = productInDb.Id;

                            if (productCreate.Prices.Any() && productCreate.Prices != null)
                            {
                                foreach (var price in productCreate.Prices)
                                {
                                    price.ProductId = productInDb.Id;
                                }
                            }

                            if (!EqualProductWithProductCreate(productInDb, productCreate, parameters))
                            {
                                var res = await this.ProductUpdate(productCreate.Id, productCreate);
                                if (res.Item1 > 0)
                                {
                                    result.ProductsUpdateSuccess++;
                                }
                                else result.ProductsUpdateErrors++;
                            }
                            else
                            {
                                // don't update, product not changed
                                result.ProductsNotUpdated++;
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Import file error: {0}", e);
                return null;
            }
            return result;
        }

        private async Task<(int, Product)> ProductCreate(ProductCreateDto productToCreate)
        {
            var product = _mapper.Map<ProductCreateDto, Product>(productToCreate);

            product.Id = _productRepository.GenerateRandomId(10);

            _unitOfWork.Repository<Product>().Add(product);

            var result = await _unitOfWork.Complete();

            return (result, product);
        }

        private async Task<(int, Product)> ProductUpdate(string id, ProductCreateDto productToUpdate)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

            // _mapper.Map(productToUpdate, product);

            product = _mapper.Map<ProductCreateDto, Product>(productToUpdate);

            _unitOfWork.Repository<Product>().Update(product);

            var result = await _unitOfWork.Complete();

            return (result, product);
        }


    }
}