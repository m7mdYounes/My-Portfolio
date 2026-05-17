using MyPortfolio.Services.Interfaces;

namespace MyPortfolio.Middleware
{
    public class AnalyticsTrackingMiddleware
    {
        private readonly RequestDelegate _next;

        public AnalyticsTrackingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            EnsureAnalyticsCookies(context);

            await _next(context);

            if (!ShouldTrackRequest(context))
                return;

            var analyticsService = context.RequestServices.GetRequiredService<IAnalyticsService>();

            var visitorKey = context.Request.Cookies[AnalyticsCookieNames.VisitorId];
            var sessionKey = context.Request.Cookies[AnalyticsCookieNames.SessionId];

            if (string.IsNullOrWhiteSpace(visitorKey) || string.IsNullOrWhiteSpace(sessionKey))
                return;

            var request = context.Request;

            var path = request.Path.Value ?? "/";
            var fullUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            var referrer = request.Headers.Referer.ToString();
            var userAgent = request.Headers.UserAgent.ToString();
            var ipAddress = GetClientIpAddress(context);

            var pageTitle = GetPageTitleFromPath(path);

            await analyticsService.TrackPageViewAsync(
                visitorKey: visitorKey,
                sessionKey: sessionKey,
                path: path,
                fullUrl: fullUrl,
                pageTitle: pageTitle,
                referrer: string.IsNullOrWhiteSpace(referrer) ? null : referrer,
                ipAddress: ipAddress,
                userAgent: string.IsNullOrWhiteSpace(userAgent) ? null : userAgent,
                cancellationToken: context.RequestAborted);
        }

        private static void EnsureAnalyticsCookies(HttpContext context)
        {
            if (!context.Request.Cookies.ContainsKey(AnalyticsCookieNames.VisitorId))
            {
                var visitorId = Guid.NewGuid().ToString("N");

                context.Response.Cookies.Append(
                    AnalyticsCookieNames.VisitorId,
                    visitorId,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = context.Request.IsHttps,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                        IsEssential = true
                    });
            }

            if (!context.Request.Cookies.ContainsKey(AnalyticsCookieNames.SessionId))
            {
                var sessionId = Guid.NewGuid().ToString("N");

                context.Response.Cookies.Append(
                    AnalyticsCookieNames.SessionId,
                    sessionId,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = context.Request.IsHttps,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                        IsEssential = true
                    });
            }
            else
            {
                var existingSessionId = context.Request.Cookies[AnalyticsCookieNames.SessionId];

                if (!string.IsNullOrWhiteSpace(existingSessionId))
                {
                    context.Response.Cookies.Append(
                        AnalyticsCookieNames.SessionId,
                        existingSessionId,
                        new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = context.Request.IsHttps,
                            SameSite = SameSiteMode.Lax,
                            Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                            IsEssential = true
                        });
                }
            }
        }

        private static bool ShouldTrackRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;

            if (!HttpMethods.IsGet(request.Method))
                return false;

            if (response.StatusCode < 200 || response.StatusCode >= 300)
                return false;

            if (AnalyticsPathFilter.ShouldIgnore(request.Path))
                return false;

            var acceptHeader = request.Headers.Accept.ToString();

            if (!string.IsNullOrWhiteSpace(acceptHeader) &&
                !acceptHeader.Contains("text/html", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private static string? GetClientIpAddress(HttpContext context)
        {
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(forwardedFor))
            {
                return forwardedFor
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault()
                    ?.Trim();
            }

            return context.Connection.RemoteIpAddress?.ToString();
        }

        private static string GetPageTitleFromPath(string path)
        {
            if (path == "/")
                return "Home";

            var segments = path
                .Trim('/')
                .Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (segments.Length == 0)
                return "Home";

            return string.Join(
                " - ",
                segments.Select(ToTitleCase));
        }

        private static string ToTitleCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            value = value.Replace("-", " ").Replace("_", " ");

            return string.Join(
                " ",
                value.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(word => char.ToUpperInvariant(word[0]) + word[1..]));
        }
    }
}
