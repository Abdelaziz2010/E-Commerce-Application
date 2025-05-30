﻿using Ecom.Application.DTOs.Auth;
using Ecom.Domain.Entities;

namespace Ecom.Application.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);
        Task<string> LoginAsync(LoginDTO loginDTO); 
        Task<bool> SendForgotPasswordEmail(string email);
        Task<bool> ActivateAccount(ActivateAccountDTO activateAccountDTO);
        Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO);
        Task<Address> GetUserAddress(string email);
        Task<bool> UpdateOrCreateAddress(string email, Address address);
        Task<AppUser> GetUserInfo(string email);
        Task<bool> DeleteUserAsync(string email);
    }
}
