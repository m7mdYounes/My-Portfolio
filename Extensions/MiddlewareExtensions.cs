using MyPortfolio.Middleware;

namespace MyPortfolio.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAnalyticsTracking(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AnalyticsTrackingMiddleware>();
        }
    }
}
