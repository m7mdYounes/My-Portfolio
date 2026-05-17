using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.ViewModels.Analytics
{
    public class TrackClickViewModel
    {
        [Required]
        [MaxLength(100)]
        public string EventType { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string ComponentName { get; set; } = null!;

        [MaxLength(100)]
        public string? TargetType { get; set; }

        [MaxLength(100)]
        public string? TargetId { get; set; }

        [MaxLength(300)]
        public string? TargetText { get; set; }

        [Required]
        [MaxLength(500)]
        public string PagePath { get; set; } = null!;

        [MaxLength(4000)]
        public string? MetadataJson { get; set; }
    }
}
