namespace MyPortfolio.Models.Analytics
{
    public class Visitor
    {
        public long Id { get; set; }

        public string VisitorKey { get; set; } = null!;

        public DateTime FirstVisitAt { get; set; } = DateTime.Now;
        public DateTime LastVisitAt { get; set; } = DateTime.Now;

        public string? FirstIpAddressHash { get; set; }
        public string? LastIpAddressHash { get; set; }

        public string? UserAgent { get; set; }
        public string? Browser { get; set; }
        public string? OperatingSystem { get; set; }
        public string? DeviceType { get; set; }

        public ICollection<VisitorSession> Sessions { get; set; } = new List<VisitorSession>();
        public ICollection<PageView> PageViews { get; set; } = new List<PageView>();
        public ICollection<ClickEvent> ClickEvents { get; set; } = new List<ClickEvent>();
    }
}
