using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;

namespace Core.Entities
{
    public class PriceListForImport
    {
        // private XmlDocument xmlDocument;
        // private string inputFileNameInStock;
        // private XmlNode rootNode;
        // private MapperConfiguration MapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<PriceType, PriceItem>());
        // private Mapper Mapper;


        // public PriceListForImport()
        // {
        //     inputFileNameInStock = "..\\..\\..\\Data\\instock.cml";
        //     xmlDocument = new XmlDocument();
        //     xmlDocument.LoadXml(File.ReadAllText(inputFileNameInStock));
        //     rootNode = xmlDocument.FirstChild.NextSibling;
        //     Mapper = new Mapper(MapperConfig);
        //     DateTime start = DateTime.UtcNow;
        //     this.Currencies = FillCurrencies();
        //     this.Categories = FillCategories();
        //     this.PriceTypes = FillPriceTypes();
        //     this.Offers = FillOfferItems();
        //     DateTime end = DateTime.UtcNow;
        //     TimeSpan timeDiff = end - start;
        //     Console.WriteLine("{0} milliseconds", timeDiff.TotalMilliseconds.ToString());
        // }

        // public Currency[] Currencies { get; set; }

        // public Price[] Prices { get; set; }

        // public OffersItem[] Offers { get; set; }

        // public PriceType[] PriceTypes { get; set; }

        // public Category[] Categories { get; set; }


        // private Currency[] FillCurrencies()
        // {
        //     return new Currency[]
        //         {
        //                 new Currency { CurrencyId = "UAH", Rate = 1, CurrencyCode = "980", Symbol="₴" },
        //                 new Currency { CurrencyId = "USD", Rate = 42, CurrencyCode = "840", Symbol = "$"},
        //                 new Currency { CurrencyId = "EUR", Rate = 45, CurrencyCode = "978", Symbol = "€" }
        //         };
        // }

        // private Category[] FillCategories()
        // {
        //     var categories = new List<Category>();
        //     Console.WriteLine("Filling categories list ...");
        //     var categoriesXml = rootNode.SelectSingleNode("Классификатор/Группы").ChildNodes;
        //     foreach (XmlNode node in categoriesXml)
        //     {
        //         Category item = new Category()
        //         {
        //             Id = node.SelectNodes("Ид").Item(0).InnerText,
        //             ParentId = node.SelectNodes("Родитель").Item(0)?.InnerText,
        //             Name = node.SelectNodes("Наименование").Item(0).InnerText
        //         };
        //         categories.Add(item);
        //     }
        //     Console.WriteLine("Filling categories list complete. Total {0}", categories.Count);

        //     return categories.ToArray();
        // }

        // private PriceType[] FillPriceTypes()
        // {
        //     Console.WriteLine("Filling price types ...");
        //     var priceTypesXml = rootNode.SelectSingleNode("ПакетПредложений/ТипыЦен").ChildNodes;
        //     var priceTypes = new List<PriceType>();
        //     foreach (XmlNode node in priceTypesXml)
        //     {
        //         PriceType item = new PriceType();
        //         if (String.Compare(node.SelectNodes("ИДЦенаСайт").Item(0).InnerText, "Розничная") == 0)
        //         {
        //             item = new PriceType()
        //             {
        //                 Id = node.SelectNodes("Ид").Item(0).InnerText,
        //                 CurrencyId = node.SelectNodes("Валюта").Item(0).InnerText,
        //                 IsRetail = true,
        //             };
        //         }
        //         else
        //         {
        //             item = new PriceType()
        //             {
        //                 Id = node.SelectNodes("Ид").Item(0).InnerText,
        //                 Quantity = Int32.Parse(Regex.Match(node.SelectNodes("Наименование").Item(0).InnerText, @"\d+").Value),
        //                 CurrencyId = node.SelectNodes("Валюта").Item(0).InnerText,
        //                 IsRetail = false,
        //             };
        //         }
        //         priceTypes.Add(item);
        //     }
        //     Console.WriteLine("Filling price types complete. Total {0}", priceTypes.Count);

        //     return priceTypes.ToArray();
        // }

        // private OffersItem[] FillOfferItems()
        // {
        //     var offers = new List<OffersItem>();

        //     Console.WriteLine("Filling product list ...");
        //     var offersXml = rootNode.SelectSingleNode("ПакетПредложений/Предложения").ChildNodes;
        //     foreach (XmlNode node in offersXml)
        //     {
        //         var prices = new List<PriceItem>();
        //         XmlNode pricesNodeXml = node.SelectSingleNode("Цены");
        //         foreach (XmlNode price in pricesNodeXml)
        //         {
        //             var Id = price.SelectNodes("ИдТипаЦены").Item(0).InnerText;
        //             PriceItem priceItem = Mapper.Map<PriceItem>(PriceTypes.First(p => p.Id == Id));
        //             priceItem.Price = Decimal.Parse(price.SelectNodes(" ЦенаЗаЕдиницу").Item(0).InnerText);

        //             prices.Add(priceItem);
        //         }

        //         OffersItem item = new OffersItem()
        //         {
        //             Id = node.SelectNodes("Ид").Item(0).InnerText,
        //             RetailPrice = prices.FirstOrDefault(p => p.IsRetail, null).Price,
        //             RetailPriceCurrencyId = prices.FirstOrDefault(p => p.IsRetail, null).CurrencyId,
        //             PriceItems = prices.Where(p => !p.IsRetail).Any() ? prices.Where(p => !p.IsRetail).ToArray() : null,
        //             SellingType = prices.Where(p => !p.IsRetail).Any() ? "u" : null

        //         };
        //         offers.Add(item);
        //     }
        //     Console.WriteLine("Filling product list complete. Total {0}", offers.Count);
        //     // fill offers list --end

        //     // fill goods list --start
        //     Console.WriteLine("Filling product characteristics ...");
        //     var goodsXml = rootNode.SelectSingleNode("ПакетПредложений/Товары").ChildNodes;
        //     var updatedOffers = new List<OffersItem>();

        //     DateTime start = DateTime.UtcNow;

        //     var keywords = new FillKeywords().Keywords;
        //     var counter = 0;
        //     foreach (XmlNode node in goodsXml)
        //     {
        //         counter++;

        //         OffersItem item = offers.FirstOrDefault(o => o.Id == node.SelectNodes("Ид").Item(0).InnerText);
        //         item.Name = node.SelectNodes("Наименование")?.Item(0)?.InnerText ?? "";
        //         item.Description = node.SelectNodes("Описание")?.Item(0)?.InnerText;
        //         item.CategoryId = node.SelectNodes("Группы/Ид").Item(0).InnerText ?? "";
        //         item.BarCode = node.SelectNodes("КодТовара").Item(0).InnerText;

        //         // keywords
        //         //if (keywords.Any())
        //         //{
        //         //    item.SearchStrings = keywords.First(kw => kw.Id == item.Id).Keys;
        //         //}
        //         //

        //         var parametersXml = node.SelectNodes("ЗначенияСвойств/ЗначенияСвойства");
        //         if (parametersXml != null)
        //         {
        //             var parameters = new List<ParameterItem>();
        //             foreach (XmlNode paramNode in parametersXml)
        //             {
        //                 ParameterItem parameter = new ParameterItem()
        //                 { // TODO: пока что параметры (свойства товаров) не используются, кроме "наличие" и "остаток"
        //                     Id = paramNode.SelectNodes("Ид").Item(0).InnerText,
        //                     Name = paramNode.SelectNodes("Наименование").Item(0).InnerText,
        //                     Value = paramNode.SelectNodes("Значение").Item(0).InnerText
        //                 };
        //                 parameters.Add(parameter);
        //             }
        //             item.Available = "true"; // bool.Parse(parameters.Find(p => p.Id == "ИД-Наличие").Value).ToString();
        //             item.Presence = "available";
        //             item.QuantityInStock = Int32.Parse(parameters.First(p => p.Id == "ИД-Количество").Value);
        //             var pars = parameters.Where(p => String.Compare(p.Name, "Количество") != 0 && String.Compare(p.Name, "Наличие") != 0);
        //             if (pars.Any())
        //             {
        //                 item.Parameters = pars.ToArray();
        //             }
        //         }

        //         Console.WriteLine("Loaded {0} of {1}, {2} {3}", counter, goodsXml.Count, item.BarCode, item.Name);

        //         updatedOffers.Add(item);
        //     }
        //     offers = updatedOffers;

        //     Console.WriteLine("Filling product characteristics complete.");

        //     return offers.ToArray();
        // }
    }

}

