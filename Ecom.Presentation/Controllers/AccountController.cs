﻿using AutoMapper;
using Ecom.Application.DTOs.Auth;
using Ecom.Application.DTOs.Order;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Domain.Entities;
using Ecom.Presentation.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

            Response.Cookies.Append("AccessToken", result, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true,
                IsEssential = true,
                Domain = "localhost",
                Expires = DateTime.UtcNow.AddDays(1)
            });

            return Ok(new ResponseAPI(200));
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            try
            {
                Response.Cookies.Append("AccessToken", "", new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict, // Use Strict for better security
                    Secure = true,
                    IsEssential = true,
                    Domain = "localhost",
                    Expires = DateTime.UtcNow.AddDays(-1)   // Expire the cookie
                });

                return Ok(new ResponseAPI(200, "Logged out successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400));
            }
        }

        [Authorize]
        [HttpGet("Get-User-Info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new ResponseAPI(400));
            }

            var user = await work.AuthRepository.GetUserInfo(email);

            if (user is null)
            {
                return NotFound(new ResponseAPI(404));
            }
            
            var userDTO = mapper.Map<UserDTO>(user);

            return Ok(userDTO);
        }
        
        [Authorize]
        [HttpGet("Get-User-Name")]
        public IActionResult GetUserName()
        {
            var name = User.Identity?.Name;

            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(new ResponseAPI(400));
            }

            return Ok(name);
        }

        [HttpPost("Activate-Account")]
        public async Task<IActionResult> ActivateAccount(ActivateAccountDTO accountDTO)
        {

            var result = await work.AuthRepository.ActivateAccount(accountDTO);

            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(400));

        }

        [HttpPost("Send-Forgot-Password-Email")]
        public async Task<IActionResult> ForgotPassword(string email) 
        {

            var result = await work.AuthRepository.SendForgotPasswordEmail(email);

            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(400));
        }

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var result = await work.AuthRepository.ResetPassword(resetPasswordDTO);
            
            if (result == "Done")
            {
                return Ok(new ResponseAPI(200));
            }

            return BadRequest(new ResponseAPI(400, result));
        }

        [Authorize]
        [HttpGet("Get-User-Address")]
        public async Task<IActionResult> GetUserAddress()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new ResponseAPI(400));
            }
            
            var address = await work.AuthRepository.GetUserAddress(email);
            
            if (address is null)
            {
                return NotFound(new ResponseAPI(404));
            }

            var addressDTO = mapper.Map<ShippingAddressDTO>(address);

            return Ok(addressDTO);
        }

        [Authorize]
        [HttpPut("Update-Or-Create-Address")]
        public async Task<IActionResult> UpdateOrCreateAddress(ShippingAddressDTO shippingAddressDTO)
        {
            if (shippingAddressDTO is null)
            {
                return BadRequest(new ResponseAPI(400));
            }

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new ResponseAPI(400));
            }

            var address = mapper.Map<Address>(shippingAddressDTO);

            var result = await work.AuthRepository.UpdateOrCreateAddress(email, address);
            
            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(400));
        }

        [HttpGet("Is-User-Authenticated")]
        public IActionResult IsUserAuthenticated()
        {
            // Check if the user is authenticated.
            return User.Identity.IsAuthenticated ? Ok(new ResponseAPI(200)) : Unauthorized(new ResponseAPI(401));
        }

        [Authorize]
        [HttpDelete("Delete-User")]
        public async Task<IActionResult> DeleteUser()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
           
            if (string.IsNullOrEmpty(email))
            {
                return NotFound(new ResponseAPI(404));
            }

            var result = await work.AuthRepository.DeleteUserAsync(email);

            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(400));
        }
    }
}
