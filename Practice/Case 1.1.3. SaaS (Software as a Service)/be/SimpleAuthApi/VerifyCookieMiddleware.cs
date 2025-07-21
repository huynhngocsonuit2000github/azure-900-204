namespace SimpleAuthApi
{
    public class VerifyCookieMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<VerifyCookieMiddleware> _logger;

        public VerifyCookieMiddleware(RequestDelegate next, ILogger<VerifyCookieMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var cookieHeader = context.Request.Headers["Cookie"].ToString();

            // Log cookies
            // _logger.LogInformation("Incoming Cookies: {Cookies}", cookieHeader);

            // User is authenticated, continue to next middleware
            await _next(context);
        }
    }

}