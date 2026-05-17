namespace MyPortfolio.ViewModels.Public
{
    public class ExperienceViewModel
    {
        public int Id { get; set; }

        public string CompanyName { get; set; } = null!;

        public string JobTitle { get; set; } = null!;

        public string? EmploymentType { get; set; }

        public string? Location { get; set; }

        public string? CompanyLogoPath { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsCurrent { get; set; }

        public string Period { get; set; } = null!;

        public List<string> Responsibilities { get; set; } = new();
    }
}
