// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Core.Interfaces;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;

// namespace Core.Entities.PriceListAggregate
// {
//     public class ImportFileProcessor
//     {
//         private readonly IProductRepository _productRepository;
//         private readonly IUnitOfWork _unitOfWork;

//         public ImportFileProcessor(IProductRepository productRepository, IUnitOfWork unitOfWork)
//         {
//             _unitOfWork = unitOfWork;
//             _productRepository = productRepository;
//         }

//         public async Task<ImportFileResult> Process(IFormFile importFile) 
//         {
//             var result = new ImportFileResult();

//             var importParameters = new ImportFileParameters(); // TODO: add logics for parameters, parameters should come together with import file

//             try
//             {
//                 long size = importFile.Length;

//                 // read file
//                 var file = await _productRepository.SaveToDiskAsync(importFile);

//                 if (file == null) return new ImportFileResultDto(false, null);

//                 var priceList = new PriceListForImport();

//                 if (await priceList.Import(file) == true)
//                 {
//                     result.ProductsTotal = priceList.Offers.Count();

//                     var categoriesInDb = await _unitOfWork.Repository<ProductCategory>().ListAllAsync();

//                     foreach (var category in priceList.Categories)
//                     {

//                         ProductCategory categoryInDb = categoriesInDb.FirstOrDefault(c => c.Id == category.Id) ?? null;
//                         if (categoryInDb is null)
//                         {
//                             // create new category and save it to DB
//                             _unitOfWork.Repository<ProductCategory>().Add(category);

//                             var res = await _unitOfWork.Complete();

//                             if (res <= 0) result.CategoriesCreateErrorsCount++;
//                             else result.CategoriesCreated++;
//                         }
//                         else
//                         {
//                             if (category == categoryInDb)
//                             {
//                                 // don't update, category not changed
//                                 result.CategoriesNotUpdated++;
//                             }
//                             else
//                             {
//                                 // category changed, update
//                                 var res = await this.UpdateCategory(category);

//                                 if (res.Result is OkObjectResult okObjectResult && okObjectResult.StatusCode == 200)
//                                 {
//                                     result.CategoriesUpdateSuccessCount++;
//                                 }
//                                 else result.CategoriesUpdateErrorsCount++;
//                             }
//                         }
//                     }

//                     // import changed products to db
//                     var productsInDb = await _unitOfWork.Repository<Product>().ListAllAsync();

//                     // find a list of products presenting in DB but not presenting in import file
//                     var notFoundProducts = new List<Product>(productsInDb.Where(p => priceList.Offers.All(offer => offer.Id != p.ExternalId)));
//                     result.ProductsNotFound = notFoundProducts.Count;

//                     foreach (var offer in priceList.Offers)
//                     {
//                         var productCreate = _mapper.Map<OfferItem, ProductCreateDto>(offer);
//                         productCreate.ProductBrandId = "1";
//                         productCreate.ProductTypeId = "1";
//                         if (string.IsNullOrEmpty(productCreate.Description)) productCreate.Description = "";

//                         Product productInDb = productsInDb.FirstOrDefault(x => x.ExternalId == productCreate.ExternalId) ?? null;

//                         if (productInDb is null) // no such product in db
//                         {
//                             // create new product and save it to DB
//                             var res = await this.CreateProduct(productCreate);
//                             if (res.Result is OkObjectResult okObjectResult && okObjectResult.StatusCode == 200)
//                             {
//                                 result.ProductsCreated++;
//                             }
//                             else result.ProductsCreateErrorsCount++;
//                         }
//                         else // product exists 
//                         {
//                             productCreate.Id = productInDb.Id;

//                             if (!EqualProductWithProductCreate(productInDb, productCreate))
//                             {
//                                 //TODO: update comparision after price type is updated to 'Price' with array of prices
//                                 var res = await this.UpdateProduct(productCreate.Id, productCreate);
//                                 if (res.Result is OkObjectResult okObjectResult && okObjectResult.StatusCode == 200)
//                                 {
//                                     result.ProductsUpdateSuccessCount++;
//                                 }
//                                 else result.ProductsUpdateErrorsCount++;
//                             }
//                             else
//                             {
//                                 // don't update, product not changed
//                                 result.ProductsNotUpdated++;
//                             }
//                         }
//                     }
//                 }
//                 else 
//                 {
//                     throw new Exception();
//                 }
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine("Import file error: {0}", e);
//                 return new ImportFileResultDto(false, null);
//             }
//             return new ImportFileResultDto(true, result);
//         }
//     }
// }