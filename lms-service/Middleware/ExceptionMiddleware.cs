using System;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace lms_service.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;

            string message = exception.Message;

            // You can customize based on exception type
            if (exception is KeyNotFoundException)
                status = HttpStatusCode.NotFound;

            if (exception is InvalidOperationException)
                status = HttpStatusCode.BadRequest;

            var response = new
            {
                success = false,
                message,
                statusCode = (int)status
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

