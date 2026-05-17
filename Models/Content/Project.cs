namespace MyPortfolio.Models.Content
{
    public class Project
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;

        public string ShortDescription { get; set; } = null!;
        public string? FullDescription { get; set; }

        public string? ProblemStatement { get; set; }
        public string? SolutionOverview { get; set; }
        public string? TechnicalHighlights { get; set; }

        public string? MainImagePath { get; set; }

        public string? DemoUrl { get; set; }
        public string? GitHubUrl { get; set; }

        public bool IsFeatured { get; set; }
        public bool IsPublished { get; set; } = true;

        public int DisplayOrder { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<ProjectImage> Images { get; set; } = new List<ProjectImage>();
        public ICollection<ProjectSkill> ProjectSkills { get; set; } = new List<ProjectSkill>();
    }
}
