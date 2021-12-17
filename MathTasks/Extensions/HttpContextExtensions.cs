using Microsoft.AspNetCore.Http;

namespace MathTasks.Extensions
{
    public static class HttpContextExtensions
    {
        private static readonly string _rootPath = "~/";
        public static string GetReturnUrl(this HttpContext httpContext) =>
            string.IsNullOrWhiteSpace(httpContext.Request.Path)
                ? _rootPath
                : $"~{httpContext.Request.Path.Value}{httpContext.Request.QueryString}";
    }
}
