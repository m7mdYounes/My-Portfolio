using MyPortfolio.Data;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.Services.Models.Analytics;

namespace MyPortfolio.Services.Implementations
{
    public class AnalyticsReportService : IAnalyticsReportService
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AnalyticsDashboardViewModel> GetDashboardAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default)
        {
            var from = fromDate?.Date ?? DateTime.UtcNow.Date.AddDays(-30);
            var to = toDate?.Date.AddDays(1) ?? DateTime.UtcNow.Date.AddDays(1);

            var pageViewsQuery = _context.PageViews
                .AsNoTracking()
                .Where(x => x.CreatedAt >= from && x.CreatedAt < to);

            var clicksQuery = _context.ClickEvents
                .AsNoTracking()
                .Where(x => x.CreatedAt >= from && x.CreatedAt < to);

            var totalPageViews = await pageViewsQuery.CountAsync(cancellationToken);

            var uniqueVisitors = await pageViewsQuery
                .Select(x => x.VisitorId)
                .Distinct()
                .CountAsync(cancellationToken);

            var totalSessions = await pageViewsQuery
                .Select(x => x.VisitorSessionId)
                .Distinct()
                .CountAsync(cancellationToken);

            var totalClicks = await clicksQuery.CountAsync(cancellationToken);

            var cvDownloads = await clicksQuery
                .CountAsync(x => x.EventType == "CvDownload", cancellationToken);

            var contactClicks = await clicksQuery
                .CountAsync(x =>
                    x.EventType.Contains("Contact") ||
                    x.ComponentName.Contains("Contact"),
                    cancellationToken);

            var projectClicks = await clicksQuery
                .CountAsync(x =>
                    x.EventType.Contains("Project") ||
                    x.TargetType == "Project",
                    cancellationToken);

            var topPages = await pageViewsQuery
                .GroupBy(x => x.Path)
                .Select(g => new PageAnalyticsItemViewModel
                {
                    Path = g.Key,
                    Views = g.Count(),
                    UniqueVisitors = g.Select(x => x.VisitorId).Distinct().Count()
                })
                .OrderByDescending(x => x.Views)
                .Take(10)
                .ToListAsync(cancellationToken);

            var topClicks = await clicksQuery
                .GroupBy(x => new { x.EventType, x.ComponentName })
                .Select(g => new ClickAnalyticsItemViewModel
                {
                    EventType = g.Key.EventType,
                    ComponentName = g.Key.ComponentName,
                    Clicks = g.Count()
                })
                .OrderByDescending(x => x.Clicks)
                .Take(10)
                .ToListAsync(cancellationToken);

            var dailyViews = await pageViewsQuery
                .GroupBy(x => x.CreatedAt.Date)
                .Select(g => new DailyAnalyticsItemViewModel
                {
                    Date = g.Key,
                    Views = g.Count(),
                    UniqueVisitors = g.Select(x => x.VisitorId).Distinct().Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync(cancellationToken);

            return new AnalyticsDashboardViewModel
            {
                TotalPageViews = totalPageViews,
                UniqueVisitors = uniqueVisitors,
                TotalSessions = totalSessions,
                TotalClicks = totalClicks,
                CvDownloads = cvDownloads,
                ContactClicks = contactClicks,
                ProjectClicks = projectClicks,
                TopPages = topPages,
                TopClicks = topClicks,
                DailyViews = dailyViews
            };
        }
    }
}
