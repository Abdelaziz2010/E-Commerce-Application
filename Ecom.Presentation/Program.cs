using Asp.Versioning.ApiExplorer;
using Ecom.Application.Extensions;
using Ecom.Infrastructure.Extensions;
using Ecom.Presentation.Extensions;
using Ecom.Presentation.Helpers;
using Ecom.Presentation.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

namespace Ecom.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Configure Serilog, used to log during startup (before the app is built)
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration) // Reads from appsettings.json
                .CreateLogger();

            builder.Host.UseSerilog();

            Log.Information("Starting the Ecom API.....");

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


            #region Health Check

            // Top-level route mapping for health checks
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckResponseWriter.WriteJsonResponse
            });


            // Liveness probe, Purpose: Tells if the app is running.
            app.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = _ => false, // No specific checks, just indicates the app is live
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var json = new
                    {
                        status = report.Status.ToString(),
                        description = "Liveness check - the app is up"
                    };
                    await context.Response.WriteAsJsonAsync(json);
                }
            });


            // Readiness probe, Tells if the app is ready to handle requests
            app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("ready"), // Only run checks tagged as "ready"
                ResponseWriter = HealthCheckResponseWriter.WriteJsonResponse
            });

            #endregion

            // Enable CORS
            app.UseCors("CORSPolicy");
            
            app.UseHttpsRedirection();

            // app.UseMiddleware<RateLimitingMiddleware>();

            app.UseMiddleware<SecurityHeadersMiddleware>();

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

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
