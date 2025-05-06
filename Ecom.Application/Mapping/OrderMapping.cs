using AutoMapper;
using Ecom.Application.DTOs.Order;
using Ecom.Domain.Entities.Orders;
namespace Ecom.Application.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping() 
        {
            CreateMap<Order, OrderToReturnDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<ShippingAddress, ShippingAddressDTO>().ReverseMap();
        }
    }
}
