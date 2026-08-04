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

        // الـ RequestDelegate ده اللي بينقل الطلب للمحطة اللي بعدها في السيرفر
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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
                // 1. تسجيل الخطأ الحقيقي للمطورين
                _logger.LogError(ex, ex.Message);

                // 2. إرجاع رسالة شيك لليوزر
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "حدث خطأ داخلي في الخادم، يرجى المحاولة لاحقاً."
                };

                // تحويل الرد لـ JSON وبعته
                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}