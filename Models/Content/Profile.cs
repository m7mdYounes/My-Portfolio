namespace MyPortfolio.Models.Content
{
    public class Profile
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;
        public string Title { get; set; } = null!;

        public string ShortSummary { get; set; } = null!;
        public string? LongSummary { get; set; }

        public string? ProfileImagePath { get; set; }
        public string? CvFilePath { get; set; }

        public string? Location { get; set; }
        public int YearsOfExperience { get; set; }

        public bool IsAvailableForWork { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
