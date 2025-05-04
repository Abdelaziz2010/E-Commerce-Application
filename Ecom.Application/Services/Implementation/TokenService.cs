using Ecom.Application.Services.Interfaces;
using Ecom.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Permissions;
using System.Security.Cryptography;
using System.Text;
namespace Ecom.Application.Services.Implementation
{
    // Here you would implement the logic to generate a JWT token for the user
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(AppUser user)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var secretKey = _configuration["JWTSettings:SecretKey"];

            var key = Encoding.ASCII.GetBytes(secretKey);

            SigningCredentials signingCreds = new SigningCredentials
                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration["JWTSettings:Issuer"],
                Audience = _configuration["JWTSettings:Audience"],
                SigningCredentials = signingCreds,
                Expires = DateTime.UtcNow.AddMinutes(30),  // short lived token
                NotBefore = DateTime.UtcNow,
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenString = tokenHandler.WriteToken(token);
           
            return tokenString;
        }
    }
}
