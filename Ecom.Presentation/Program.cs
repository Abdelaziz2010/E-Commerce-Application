
using Ecom.Infrastructure.Extensions;
using Ecom.Application.Extensions;
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
            
            builder.Services.AddSwaggerGen();

            // Pass the configuration to AddInfrastructureServices
            
            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddApplicationServices();





            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable CORS
            app.UseCors("CORSPolicy");
            
            app.UseHttpsRedirection();

            app.UseMiddleware<RateLimitingMiddleware>();

            app.UseMiddleware<SecurityHeadersMiddleware>();

            app.UseMiddleware<ExceptionsMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
