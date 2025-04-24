using Microsoft.AspNetCore.Http;

namespace Ecom.Application.Services.Interfaces
{
    public interface IImageManagementService
    {
        Task<List<string>> AddImageAsync(IFormFileCollection files, string src);
        void DeleteImageAsync(string src);
    }
}
