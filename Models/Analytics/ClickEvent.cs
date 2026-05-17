namespace MyPortfolio.Models.Analytics
{
    public class ClickEvent
    {
        public long Id { get; set; }

        public long VisitorId { get; set; }
        public Visitor Visitor { get; set; } = null!;

        public long VisitorSessionId { get; set; }
        public VisitorSession VisitorSession { get; set; } = null!;

        public string EventType { get; set; } = null!;
        public string ComponentName { get; set; } = null!;

        public string? TargetType { get; set; }
        public string? TargetId { get; set; }
        public string? TargetText { get; set; }

        public string PagePath { get; set; } = null!;
        public string? MetadataJson { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
