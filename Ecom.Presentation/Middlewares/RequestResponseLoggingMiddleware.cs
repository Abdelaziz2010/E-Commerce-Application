using Serilog;

namespace Ecom.Presentation.Middlewares
{
    // Logs incoming requests (method, path, query).
    // Logs outgoing response status code.
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            // Log Request Details
            Log.Information("Incoming Request: {Method} {Path} {QueryString}",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString);

            // Copy original response body to capture the response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context); // Proceed to next middleware

            // Log Response Details
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            Log.Information("Outgoing Response: {StatusCode}", context.Response.StatusCode);

            await responseBody.CopyToAsync(originalBodyStream); // Copy back to the original stream
        }
    }
}
