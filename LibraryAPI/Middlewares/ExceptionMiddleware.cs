using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        // الـ RequestDelegate ده اللي بينقل الطلب للمحطة اللي بعدها في السيرفر
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger , IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // لو مفيش مشكلة، الطلب بيكمل طريقه للكنترولر عادي
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // لو إحنا في مرحلة التطوير، اظهر الخطأ الحقيقي، غير كده اظهر الرسالة العامة
                object response;
                if (_env.IsDevelopment())
                {
                    response = (new { StatusCode = context.Response.StatusCode, Message = ex.Message, Details = ex.StackTrace });
                }
                else
                {
                    response = (new { StatusCode = context.Response.StatusCode, Message = "حدث خطأ داخلي في الخادم، يرجى المحاولة لاحقاً." });
                }

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
