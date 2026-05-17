namespace MyPortfolio.Models.Analytics
{
    public class PageView
    {
        public long Id { get; set; }

        public long VisitorId { get; set; }
        public Visitor Visitor { get; set; } = null!;

        public long VisitorSessionId { get; set; }
        public VisitorSession VisitorSession { get; set; } = null!;

        public string Path { get; set; } = null!;
        public string? FullUrl { get; set; }
        public string? PageTitle { get; set; }
        public string? Referrer { get; set; }

        public string? IpAddressHash { get; set; }
        public string? UserAgent { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
