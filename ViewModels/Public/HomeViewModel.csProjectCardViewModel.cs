namespace MyPortfolio.ViewModels.Public
{
    public class HomeViewModel
    {
        public ProfileSummaryViewModel? Profile { get; set; }

        public List<ExperienceViewModel> Experiences { get; set; } = new();

        public List<ProjectCardViewModel> FeaturedProjects { get; set; } = new();

        public List<SkillCategoryViewModel> SkillCategories { get; set; } = new();

        public List<ContactLinkViewModel> ContactLinks { get; set; } = new();
    }

    public class ProfileSummaryViewModel
    {
        public string FullName { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string ShortSummary { get; set; } = null!;

        public string? LongSummary { get; set; }

        public string? ProfileImagePath { get; set; }

        public string? CvFilePath { get; set; }

        public string? Location { get; set; }

        public int YearsOfExperience { get; set; }

        public bool IsAvailableForWork { get; set; }
    }

    public class ProjectCardViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string ShortDescription { get; set; } = null!;

        public string? MainImagePath { get; set; }

        public bool IsFeatured { get; set; }

        public List<string> Skills { get; set; } = new();
    }
}
