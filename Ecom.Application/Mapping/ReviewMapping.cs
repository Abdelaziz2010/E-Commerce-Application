using AutoMapper;
using Ecom.Application.DTOs.Reviews;
using Ecom.Domain.Entities.Product;

namespace Ecom.Application.Mapping
{
    public class ReviewMapping : Profile
    {
        public ReviewMapping()
        {
            CreateMap<Review, ReturnReviewDTO>()
                .ForMember(dest => dest.UserName, op => op.MapFrom(src => src.AppUser.UserName));

            CreateMap<ReviewDTO, Review>();
        }
    }
}
