using Ecom.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Ecom.Application.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(AppUser user);
        RefreshToken GenerateRefreshToken(string userId);
        Task SetRefreshToken(RefreshToken newRefreshToken, HttpResponse response);
        Task RevokeRefreshToken(string refreshToken);
        Task RevokeAllRefreshTokens(string userId);
    }
}
