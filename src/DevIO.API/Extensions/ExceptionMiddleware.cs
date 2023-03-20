using System.Net;

namespace DevIO.API.Extensions
{
    // garantir que qualquer erro seja tratado

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(httpContext, ex);
            }
        }

        private static void HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //forma utilizada com o elmah
            // exception.Ship(context);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        }
    }
}
