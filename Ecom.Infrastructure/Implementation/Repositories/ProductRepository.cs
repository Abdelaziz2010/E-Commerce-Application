using AutoMapper;
using Ecom.Application.DTOs;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Application.Services.Interfaces;
using Ecom.Domain.Entities.Product;
using Ecom.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Implementation.Repositories
{
    public class ProductRepository: GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;

        public ProductRepository(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }

        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            
            if (productDTO is null)
            {
                return false;
            }

            var product = mapper.Map<Product>(productDTO);
          
            await context.Products.AddAsync(product);

            await context.SaveChangesAsync();


            //save image

            var ImagePath = await imageManagementService.AddImageAsync(productDTO.Photos, productDTO.Name);

            var photos = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id
            }).ToList();

            await context.Photos.AddRangeAsync(photos);

            await context.SaveChangesAsync();

            return true;

        }
    }
}
