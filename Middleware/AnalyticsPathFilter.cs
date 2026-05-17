namespace MyPortfolio.Middleware
{
    public static class AnalyticsPathFilter
    {
        private static readonly string[] IgnoredPrefixes =
        [
            "/admin",
        "/analytics",
        "/css",
        "/js",
        "/lib",
        "/images",
        "/uploads"
        ];

        private static readonly string[] IgnoredExactPaths =
        [
            "/favicon.ico",
        "/robots.txt",
        "/sitemap.xml"
        ];

        private static readonly string[] IgnoredExtensions =
        [
            ".css",
        ".js",
        ".png",
        ".jpg",
        ".jpeg",
        ".webp",
        ".gif",
        ".svg",
        ".ico",
        ".woff",
        ".woff2",
        ".ttf",
        ".map"
        ];

        public static bool ShouldIgnore(PathString path)
        {
            if (!path.HasValue)
                return true;

            var value = path.Value!.ToLowerInvariant();

            if (IgnoredExactPaths.Contains(value))
                return true;

            if (IgnoredPrefixes.Any(prefix => value.StartsWith(prefix)))
                return true;

            if (IgnoredExtensions.Any(extension => value.EndsWith(extension)))
                return true;

            return false;
        }
    }
}
