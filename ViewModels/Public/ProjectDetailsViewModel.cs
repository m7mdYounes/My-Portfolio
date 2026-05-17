namespace MyPortfolio.ViewModels.Public
{
    public class ProjectDetailsViewModel
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

        public List<string> Images { get; set; } = new();

        public List<string> Skills { get; set; } = new();
    }
}
