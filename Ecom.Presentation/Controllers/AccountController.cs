using AutoMapper;
using Ecom.Application.DTOs.Auth;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Presentation.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Presentation.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {

            var result = await work.AuthRepository.RegisterAsync(registerDTO);

            if (result != "Done")
            {
                return BadRequest(new ResponseAPI(400, result));
            }

            return Ok(new ResponseAPI(200, result));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {

            string result = await work.AuthRepository.LoginAsync(loginDTO);
           
            if (result == null)
            {
                return BadRequest(new ResponseAPI(400));
            }

            if(result.StartsWith("Please"))
            {
                return BadRequest(new ResponseAPI(400, result));
            }

            Response.Cookies.Append("accesstoken", result, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                IsEssential = true,
                Domain = "localhost",
                Expires = DateTimeOffset.UtcNow.AddMinutes(30)
            });

            return Ok(new ResponseAPI(200));
        }

        [HttpPost("Activate-Account")]
        public async Task<IActionResult> ActivateAccount(ActiveAccountDTO accountDTO)
        {

            var result = await work.AuthRepository.ActivateAccount(accountDTO);

            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(200));
        }

        [HttpPost("Send-Email-Forget-Password")]
        public async Task<IActionResult> ForgetPassword(string email)
        {

            var result = await work.AuthRepository.SendEmailForForgetPassword(email);

            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(200));
        }

    }
}
