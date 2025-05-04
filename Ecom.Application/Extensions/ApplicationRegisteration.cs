using Ecom.Application.Services.Implementation;
using Ecom.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Ecom.Application.Extensions
{
    public static class ApplicationRegisteration 
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register the AutoMapper service
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Register the IFileProvider for the ImageManagementService
            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            // Register Image Service
            services.AddSingleton<IImageManagementService, ImageManagementService>();

            // Register Email Service
            services.AddScoped<IEmailService, EmailService>();

            // Register Token Service
            // services.AddScoped<ITokenService, TokenService>();

            return services;
        }

    }
}
