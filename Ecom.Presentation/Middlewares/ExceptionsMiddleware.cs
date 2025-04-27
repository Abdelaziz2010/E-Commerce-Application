using Ecom.Presentation.Helpers;
using System.Net;
using System.Text.Json;

namespace Ecom.Presentation.Middlewares
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment environment = null)
        {
            _next = next;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _environment.IsDevelopment() ?
                    new ProblemDetail((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                  : new ProblemDetail((int)HttpStatusCode.InternalServerError, ex.Message);

                var json = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);

            }
        }

    }
}
