using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.ViewModels.Admin
{
    public class ExperienceFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string JobTitle { get; set; } = null!;

        [MaxLength(100)]
        public string? EmploymentType { get; set; }

        [MaxLength(150)]
        public string? Location { get; set; }

        public string? CurrentCompanyLogoPath { get; set; }

        public IFormFile? CompanyLogo { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsCurrent { get; set; }

        public bool IsPublished { get; set; } = true;

        public int DisplayOrder { get; set; }

        public List<string> Responsibilities { get; set; } = new();
    }
}
