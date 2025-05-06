using AutoMapper;
using Ecom.Application.DTOs.Order;
using Ecom.Domain.Entities.Orders;
namespace Ecom.Application.Mapping
{
    public class DeliveryMethodMapping : Profile
    {
        public DeliveryMethodMapping()
        {
            CreateMap<DeliveryMethod, DeliveryMethodDTO>().ReverseMap();
        }
    }
}
