using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using Core.Entities.PriceListAggregate;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.ProductCategory, o => o.MapFrom(s => s.ProductCategory.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>())
                ;

            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();

            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<CustomerBasketDto, CustomerBasket>();

            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.Value));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());

            CreateMap<ProductCreateDto, Product>();

            CreateMap<OfferItem, Product>()
                .ForMember(d => d.Price, o => o.MapFrom(s => s.RetailPrice))
                .ForMember(d => d.Stock, o => o.MapFrom(s => s.QuantityInStock))
                .ForMember(d => d.ProductCategoryId, o => o.MapFrom(s => s.CategoryId))
                .ForMember(d => d.ExternalId, o => o.MapFrom(s => s.Id))
                ;

            CreateMap<OfferItem, ProductCreateDto>()
                .ForMember(d => d.Price, o => o.MapFrom(s => s.RetailPrice))
                .ForMember(d => d.Stock, o => o.MapFrom(s => s.QuantityInStock))
                .ForMember(d => d.ProductCategoryId, o => o.MapFrom(s => s.CategoryId))
                .ForMember(d => d.ExternalId, o => o.MapFrom(s => s.Id))
                // .ForMember(d => d.Prices, o => o.MapFrom(s => s.PriceItems))
                .ForMember(d => d.Description, o=>o.NullSubstitute(string.Empty))
                .AfterMap((s,d) => d.ProductBrandId = "1") // TODO: update logics after type and brand logics is updated
                .AfterMap((s,d) => d.ProductTypeId = "1") // TODO:
                .ForMember(d => d.Id, o => o.Ignore())
                ;

            CreateMap<Photo, PhotoToReturnDto>()
                .ForMember(d => d.PictureUrl,
                    o => o.MapFrom<PhotoUrlResolver>());
                    
            CreateMap<AppUser, UserDto>()
                .ForMember(u => u.Email, r => r.MapFrom(s => s.Id))
                //               .ForMember(u=>u.Email, r=>MapFrom<userRolesResolver>())
                ;

        }
    }
}