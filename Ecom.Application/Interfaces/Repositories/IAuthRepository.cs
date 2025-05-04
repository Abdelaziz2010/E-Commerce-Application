

using Ecom.Application.DTOs.Auth;

namespace Ecom.Application.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);
        Task<string> LoginAsync(LoginDTO loginDTO); 
        Task<bool> SendForgotPasswordEmail(string email);
        Task<bool> ActivateAccount(ActivateAccountDTO activateAccountDTO);
        Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO);
    }
}
