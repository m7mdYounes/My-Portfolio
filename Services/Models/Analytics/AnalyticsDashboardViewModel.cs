namespace MyPortfolio.Services.Models.Analytics
{
    public class AnalyticsDashboardViewModel
    {
        public int TotalPageViews { get; set; }

        public int UniqueVisitors { get; set; }

        public int TotalSessions { get; set; }

        public int TotalClicks { get; set; }

        public int CvDownloads { get; set; }

        public int ContactClicks { get; set; }

        public int ProjectClicks { get; set; }

        public List<PageAnalyticsItemViewModel> TopPages { get; set; } = new();

        public List<ClickAnalyticsItemViewModel> TopClicks { get; set; } = new();

        public List<DailyAnalyticsItemViewModel> DailyViews { get; set; } = new();
    }

    public class PageAnalyticsItemViewModel
    {
        public string Path { get; set; } = null!;

        public int Views { get; set; }

        public int UniqueVisitors { get; set; }
    }

    public class ClickAnalyticsItemViewModel
    {
        public string EventType { get; set; } = null!;

        public string ComponentName { get; set; } = null!;

        public int Clicks { get; set; }
    }

    public class DailyAnalyticsItemViewModel
    {
        public DateTime Date { get; set; }

        public int Views { get; set; }

        public int UniqueVisitors { get; set; }
    }
}
