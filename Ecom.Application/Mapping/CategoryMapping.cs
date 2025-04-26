using AutoMapper;
using Ecom.Application.DTOs.Category;
using Ecom.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Application.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping() 
        {
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<UpdateCategoryDTO, Category>().ReverseMap();
        
        }

    }
}
