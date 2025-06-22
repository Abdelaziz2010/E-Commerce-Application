
using Asp.Versioning.ApiExplorer;
using Ecom.Application.Extensions;
using Ecom.Infrastructure.Extensions;
using Ecom.Presentation.Extensions;
using Ecom.Presentation.Helpers;
using Ecom.Presentation.Middlewares;

namespace Ecom.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Add CORS configurations policy to allow requests from the client app

            builder.Services.AddCors(op =>
            {
                op.AddPolicy("CORSPolicy",builder =>
                {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials()
                           .WithOrigins("http://localhost:4200", "https://localhost:4200");
                });
            });

            builder.Services.AddMemoryCache();

            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();

            // swagger
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

            builder.Services.AddSingleton<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();

            // Pass the configuration to AddInfrastructureServices
            
            builder.Services.AddPresentationServices(builder.Configuration);

            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddApplicationServices();





            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                app.UseSwagger();
                
                app.UseSwaggerUI(options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                                description.GroupName.ToUpperInvariant());
                    }
                });
            }

            // Enable CORS
            app.UseCors("CORSPolicy");
            
            app.UseHttpsRedirection();

            app.UseMiddleware<RateLimitingMiddleware>();

            app.UseMiddleware<SecurityHeadersMiddleware>();

            app.UseMiddleware<ExceptionsMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            // Use Rate Limiter globally
            app.UseRateLimiter();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
