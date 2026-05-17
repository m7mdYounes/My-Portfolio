namespace MyPortfolio.Services.Models.Analytics
{
    public class TrackClickRequest
    {
        public string EventType { get; set; } = null!;

        public string ComponentName { get; set; } = null!;

        public string? TargetType { get; set; }

        public string? TargetId { get; set; }

        public string? TargetText { get; set; }

        public string PagePath { get; set; } = null!;

        public string? MetadataJson { get; set; }
    }
}
