using Kanbersky.Uploader.Core.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Kanbersky.Uploader.Core.Extensions
{
    public static class RegistrationExtensions
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
