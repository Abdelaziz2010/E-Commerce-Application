using AutoMapper;
using Ecom.Application.DTOs.Order;
using Ecom.Domain.Entities;
using Ecom.Domain.Entities.Orders;
namespace Ecom.Application.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping() 
        {
            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(dest => dest.DeliveryMethod, op => op.MapFrom(src => src.DeliveryMethod.Name)).ReverseMap();

            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();

            CreateMap<ShippingAddress, ShippingAddressDTO>().ReverseMap();

            CreateMap<Address, ShippingAddressDTO>().ReverseMap();
        }
    }
}
