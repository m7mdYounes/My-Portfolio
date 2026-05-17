namespace MyPortfolio.Models.Analytics
{
    public class DailyAnalyticsSummary
    {
        public long Id { get; set; }

        public DateOnly Date { get; set; }

        public int TotalPageViews { get; set; }
        public int UniqueVisitors { get; set; }
        public int TotalSessions { get; set; }
        public int TotalClicks { get; set; }

        public int CvDownloads { get; set; }
        public int ContactClicks { get; set; }
        public int ProjectCardClicks { get; set; }
    }
}
