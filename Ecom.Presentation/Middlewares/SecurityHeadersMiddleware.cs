namespace Ecom.Presentation.Middlewares
{
    // Add security headers middleware
    // This middleware can be used to add security headers to the HTTP response.
    // These headers can help protect against common vulnerabilities and attacks.
    // X-Content-Type-Options: Prevents the browser from MIME-sniffing a response away from the declared content-type.
    // X-XSS-Protection: Enables the cross-site scripting (XSS) filter in the browser.
    // X-Frame-Options: Prevents the page from being displayed in a frame or iframe.
   

    public class SecurityHeadersMiddleware
    {
        readonly RequestDelegate _next;
        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Add security headers to protect against common vulnerabilities and attacks
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-XSS-Protection"] = "1;mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";
            
            await _next(context);
        }
    }
}
