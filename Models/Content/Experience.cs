namespace MyPortfolio.Models.Content
{
    public class Experience
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
        public bool IsPublished { get; set; } = true;

        public int DisplayOrder { get; set; }

        public ICollection<ExperienceResponsibility> Responsibilities { get; set; }
            = new List<ExperienceResponsibility>();
    }
}
