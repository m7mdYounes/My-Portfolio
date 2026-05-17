using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.ViewModels.Admin
{
    public class SkillFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public int SkillCategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? IconPath { get; set; }

        [MaxLength(50)]
        public string? Level { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsPublished { get; set; } = true;

        public List<SelectListItem> Categories { get; set; } = new();
    }
}
