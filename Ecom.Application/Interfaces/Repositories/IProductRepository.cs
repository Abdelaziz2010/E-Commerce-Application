using Ecom.Application.DTOs.Product;
using Ecom.Application.Shared;
using Ecom.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        // Add any additional methods specific to Product repository here, For future functionality
        
        Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParams productParams);
        Task<bool> AddAsync(AddProductDTO addProductDTO);
        Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO);
        Task DeleteAsync(Product product);

    }
}
