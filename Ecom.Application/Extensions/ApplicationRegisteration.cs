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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.AddSingleton<IImageManagementService, ImageManagementService>();

            // Register Email Service
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }

    }
}
