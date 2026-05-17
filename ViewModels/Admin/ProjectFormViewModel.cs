using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.ViewModels.Admin
{
    public class ProjectFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        public string? Slug { get; set; }

        [Required]
        [MaxLength(1000)]
        public string ShortDescription { get; set; } = null!;

        [MaxLength(8000)]
        public string? FullDescription { get; set; }

        [MaxLength(4000)]
        public string? ProblemStatement { get; set; }

        [MaxLength(4000)]
        public string? SolutionOverview { get; set; }

        [MaxLength(4000)]
        public string? TechnicalHighlights { get; set; }

        public string? CurrentMainImagePath { get; set; }

        public IFormFile? MainImage { get; set; }

        [MaxLength(1000)]
        public string? DemoUrl { get; set; }

        [MaxLength(1000)]
        public string? GitHubUrl { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsPublished { get; set; } = true;

        public int DisplayOrder { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<int> SelectedSkillIds { get; set; } = new();

        public List<SelectListItem> AvailableSkills { get; set; } = new();
    }
}
