using Ecom.Application.Interfaces.Repositories;
using Ecom.Domain.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace Ecom.Infrastructure.Implementation.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IDatabase _database;
        public CartRepository(IConnectionMultiplexer redis) 
        {
            _database = redis.GetDatabase();
        }

        public async Task<Cart> GetCartAsync(string id)
        {
            var result = await _database.StringGetAsync(id);

            if (!string.IsNullOrEmpty(result))
            {
                return JsonSerializer.Deserialize<Cart>(result);
            }
            
            return null;
        }

        // This method updates or create a shopping cart in Redis, storing it as a JSON string with a 3-day expiration
        public async Task<Cart> UpdateOrCreateCartAsync(Cart cart) 
        {

            var _cart = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(3));

            if(_cart)
            {
                return await GetCartAsync(cart.Id);
            }

            return null;
        }

        public async Task<bool> DeleteCartAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }
    }
}
