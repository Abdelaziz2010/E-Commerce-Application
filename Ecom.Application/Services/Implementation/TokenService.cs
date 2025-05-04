using Ecom.Application.Services.Interfaces;
using Ecom.Domain.Entities;
namespace Ecom.Application.Services.Implementation
{
    // Here you would implement the logic to generate a JWT token for the user
    public class TokenService : ITokenService
    {
        public Task<string> GenerateToken(AppUser user)
        {
            
            return Task.FromResult("GeneratedToken");
        }
    }
}
