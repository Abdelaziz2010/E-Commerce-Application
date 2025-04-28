using Ecom.Presentation.Helpers;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.Presentation.Middlewares
{
    // Apply Rate Limiting Middleware
    // This middleware can be used to limit the number of requests from a specific IP address
    // within a certain time frame. This is useful to prevent abuse and ensure fair usage of resources.

    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);

        public RateLimitingMiddleware(RequestDelegate next, IMemoryCache memoryCache)
        {
            _next = next;
            _memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
           
            if (IsIpAddressAllowed(context) == false)        // Check if the IP address is allowed to make requests
            {
                // If not, return a 429 Too Many Requests response

                context.Response.ContentType = "application/json";

                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                
                var response = 
                    new ProblemDetail((int)HttpStatusCode.TooManyRequests, "Too many requests, Please try again later");

                var json = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);

                return;      // Stop further middleware execution
            }

            await _next(context);                // If allowed, continue to the next middleware

        }

        private bool IsIpAddressAllowed(HttpContext context)
        {

            var ip = context.Connection.RemoteIpAddress?.ToString();

            var cachKey = $"Rate:{ip}";

            var dateNow = DateTime.Now;

            // Check if the IP address is already in the cache
            // timeStamp: the time of the last request وقت اخر ريكوست حصل 
            var (timesTamp, count) = _memoryCache.GetOrCreate(cachKey, entry =>
            {
                // If not, create a new cache entry with the current timestamp and count of 0
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;

                return (timesTamp: dateNow, count: 0);
            });

            // Check if the time since the last request is less than the rate limit window
            // If the count exceeds the limit, return false (not allowed)
            // If the time since the last request is greater than the rate limit window, reset the count and timestamp
            if (dateNow - timesTamp < _rateLimitWindow)
            {
                if (count >= 8)
                {
                    return false;
                }

                // Increment the count of requests for this IP address
                _memoryCache.Set(cachKey, (timesTamp, count += 1), _rateLimitWindow);
            }
            else
            {
                // Reset the count and timestamp for this IP address
                _memoryCache.Set(cachKey, (dateNow, count), _rateLimitWindow);
            }

            return true;
        }
    }
}
