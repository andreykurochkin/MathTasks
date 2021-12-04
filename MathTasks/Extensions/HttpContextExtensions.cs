using Microsoft.AspNetCore.Http;

namespace MathTasks.Extensions
{
    public static class HttpContextExtensions
    {
        private const string RootPath = "~/";
        public static string GetReturnUrl(this HttpContext httpContext) => string.IsNullOrWhiteSpace(httpContext.Request.Path)
            ? RootPath
            : $"~{httpContext.Request.Path.Value}{httpContext.Request.QueryString}";
    }
}
