using Ecom.Application.DTOs;
using Ecom.Application.DTOs.Auth;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Application.Services.Interfaces;
using Ecom.Application.Shared;
using Ecom.Domain.Entities;
using Microsoft.AspNetCore.Identity;
namespace Ecom.Infrastructure.Implementation.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public AuthRepository(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            IEmailService emailService, 
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _tokenService = tokenService;
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

            //Send Active Email

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            await SendEmail(user.Email, token, "Active", "Active Email", "Please click the button below to activate your account");

            return "Done";
        }

        // component variable is used to determine Component name in angular app
        public async Task SendEmail(string email, string token, string component, string subject,string message)
        {
            var result = new EmailDTO
                (email,
                "abdelazizsaleh1999@gmail.com",
                subject,
                EmailStringBody.SendEmailBody(email, token, component, message)
                );

            await _emailService.SendEmail(result);
        }

        public async Task<string> LoginAsync(LoginDTO loginDTO)
        {

            if (loginDTO is null)
            {
                return "Invalid Data";
            }

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            
            if (user is null)
            {
                return "This Email not exists";
            }

            if (user.EmailConfirmed is not true)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
               
                await SendEmail(user.Email, token, "Active", "Active Email", "Please click the button below to activate your account");

                return "Please check your email to active your account, We have sent an activation link to your E-mail";
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, false, true);

            if(result.Succeeded is not true)
            {
                return "Invalid Email or Password";
            }

            //Generate Token

            return "Done";  // return JWT Token
        }
    }
}
