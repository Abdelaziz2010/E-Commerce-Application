using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.RateLimiting;

namespace Ecom.Presentation.Extensions
{
    public static class PresentationRegisteration
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
        {

            // Add Rate Limiting globally
            #region Rate Limiting Configurations

            services.AddRateLimiter(options =>
            {
                // Policy for read-only endpoints (GET)
                options.AddPolicy("ReadOnlyPolicy", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        key => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 20, // 20 requests
                            Window = TimeSpan.FromMinutes(1), // per minute
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0   // Reject immediately after 3 requests and avoids the "frozen" effect, no tie up server resources
                        }));

                // Policy for write endpoints (POST/PUT/DELETE)
                options.AddPolicy("WritePolicy", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        key => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,  // 5 requests
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0    // Reject immediately after 3 requests and avoids the "frozen" effect.
                        }));

                // global limiter as a fallback for untagged endpoints.
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        key => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 50,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        }));

                // Custom response on rate limit rejection
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsync("{\"error\": \"You are being rate limited. Please try again later.\"}",token);
                };
            });

            #endregion

            return services;
        }
    }
}
