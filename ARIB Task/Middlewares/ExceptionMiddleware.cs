using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ARIB_Task.Middlewares
{
    public class ExceptionMiddleware(ILogger<ExceptionMiddleware> _logger, IHostEnvironment _environment) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                ProblemDetails problem = new()
                {
                    Status = context.Response.StatusCode,
                    Title = _environment.IsDevelopment() ? ex.Message : "There is an error, please contact with supporting team.",
                    Detail = _environment.IsDevelopment() ? ex.InnerException?.ToString() : null,
                };
                var jsonExp = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(jsonExp);
            }
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
