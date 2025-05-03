using Ecom.Application.DTOs.Auth;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Domain.Entities;
using Microsoft.AspNetCore.Identity;
namespace Ecom.Infrastructure.Implementation.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<AppUser> _userManager;
        public AuthRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {

            if(registerDTO is null)
            {
                return "Invalid Data";
            }

            if(await _userManager.FindByNameAsync(registerDTO.UserName) is not null)
            {
                return "This UserName already exists";
            }

            if (await _userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return "This Email already exists";
            }

            AppUser user = new AppUser()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded is not true)
            {
                return result.Errors.ToList()[0].Description;
            }

            //send active email
            //send token to user


            return "Done";
        }
    }
}
