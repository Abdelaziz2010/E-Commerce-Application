
using AutoMapper;
using Ecom.Application.DTOs.Auth;
using Ecom.Domain.Entities;

namespace Ecom.Application.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping() 
        {
            CreateMap<AppUser, UserDTO>();
        }
    }
}
