namespace MyPortfolio.Models.Analytics
{
    public class VisitorSession
    {
        public long Id { get; set; }

        public long VisitorId { get; set; }
        public Visitor Visitor { get; set; } = null!;

        public string SessionKey { get; set; } = null!;

        public DateTime StartedAt { get; set; } = DateTime.Now;
        public DateTime LastActivityAt { get; set; } = DateTime.Now;

        public string? IpAddressHash { get; set; }
        public string? UserAgent { get; set; }
        public string? Referrer { get; set; }

        public ICollection<PageView> PageViews { get; set; } = new List<PageView>();
        public ICollection<ClickEvent> ClickEvents { get; set; } = new List<ClickEvent>();
    }
}
