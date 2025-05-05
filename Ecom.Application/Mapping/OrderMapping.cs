using AutoMapper;
using Ecom.Application.DTOs.Order;
using Ecom.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Application.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping() 
        {
            CreateMap<ShippingAddress, ShippingAddressDTO>().ReverseMap();
        }
    }
}
