using AutoMapper;
using Ecom.Application.DTOs.Product;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Application.Services.Interfaces;
using Ecom.Application.Shared;
using Ecom.Domain.Entities.Product;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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


        public async Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParams productParams)
        {

            var query = context.Products
                .Include(p => p.Category)
                .Include(p => p.Photos)
                .AsNoTracking();


            // Apply Filtering/Searching by word
            if (!string.IsNullOrEmpty(productParams.Search))
            {
                var searchWords = productParams.Search.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(p => searchWords.All(word =>
                        p.Name.ToLower().Contains(word) || p.Description.ToLower().Contains(word)));

                #region Alternative approach using a loop
                //foreach (var word in searchWords)
                //{
                //    query = query.Where(p => p.Name.ToLower().Contains(word) ||
                //                             p.Description.ToLower().Contains(word));
                //} 
                #endregion

            }

            // Apply Filtering by CategoryId 
            if (productParams.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == productParams.CategoryId);
            }

            // Apply Sorting
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                query = productParams.Sort switch
                {
                    "PriceASC"  => query.OrderBy(p => p.NewPrice),
                    "PriceDESC" => query.OrderByDescending(p => p.NewPrice),
                     _  => query.OrderBy(p => p.Name),
                };
            }
            else
            {
                // Default ordering to avoid unpredictable results
                query = query.OrderBy(p => p.Id);
            }

            // Apply Pagination
            query = query.Skip((productParams.PageNumber - 1) * productParams.PageSize).Take(productParams.PageSize);

            var result = await query.ToListAsync();    // Ensure async execution

            return mapper.Map<List<ProductDTO>>(result);
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

        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO)
        {
            if (updateProductDTO is null)
            {
                return false;
            }

            var FindProduct = await context.Products
                .Include(p => p.Category).Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.Id == updateProductDTO.Id);

            if (FindProduct is null)
            {
                return false;
            }

            mapper.Map(updateProductDTO, FindProduct);

            //Update the properties of the existing product this is what mappper.Map() is doing

            //FindProduct.Name = updateProductDTO.Name;
            //FindProduct.Description = updateProductDTO.Description;
            //FindProduct.CategoryId = updateProductDTO.CategoryId;
            //FindProduct.NewPrice = updateProductDTO.NewPrice;
            //FindProduct.OldPrice = updateProductDTO.OldPrice;


            //if the user does not want to change the image, we do not need to delete the old image

            if (updateProductDTO.Photos is not null ) 
            {

                var FindPhotos = await context.Photos
                    .Where(p => p.ProductId == updateProductDTO.Id)
                    .ToListAsync();

                foreach (var photo in FindPhotos)
                {
                    imageManagementService.DeleteImageAsync(photo.ImageName);
                }

                context.Photos.RemoveRange(FindPhotos);


                var ImagePath = await imageManagementService.AddImageAsync(updateProductDTO.Photos, updateProductDTO.Name);

                var photos = ImagePath.Select(path => new Photo
                {
                    ImageName = path,
                    ProductId = updateProductDTO.Id
                }).ToList();

                await context.Photos.AddRangeAsync(photos); 
            }

            await context.SaveChangesAsync();

            return true;
        }

        public async Task DeleteAsync(Product product)
        {

            //var photos = await context.Photos.Where(p => p.ProductId == product.Id).ToListAsync();

            foreach (var photo in product.Photos)
            {
                 imageManagementService.DeleteImageAsync(photo.ImageName);
            }
            
            context.Products.Remove(product);
            
            await context.SaveChangesAsync();

        }

    }
}
