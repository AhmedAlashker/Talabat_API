using AutoMapper;
using Talabat.APIs.DTO;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product;


namespace Talabat.APIs.Helper
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {

            CreateMap<Product, ProductToReturnDto>()
                .ForMember(p => p.Brand, O => O.MapFrom(s => s.Brand.Name))
                .ForMember(p => p.Category, O => O.MapFrom(s => s.Category.Name))
                //.ForMember(p => p.PictureUrl, O => O.MapFrom(s => $"{"https://localhost:7019"}/{s.PictureUrl}"));
                .ForMember(P => P.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

            CreateMap<CustomerBasketDto,  CustomerBasket>();
            CreateMap<BasketItemDto,  BasketItem>();
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<Core.Entities.Order_Aggregate.Address, OrderAddressDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());
                


        }
    }
}
