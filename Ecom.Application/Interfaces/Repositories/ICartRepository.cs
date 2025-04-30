using Ecom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        // Retrieve a cart by its ID
        Task<Cart> GetCartAsync(string id);

        // Update or Add a cart
        Task<Cart> UpdateOrCreateCartAsync(Cart cart);
         
        // Delete a cart
        Task<bool> DeleteCartAsync(string id);
    }
}
