
using Ecom.Application.DTOs;

namespace Ecom.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(EmailDTO emailDTO);
    }
}
