using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.ViewModels.Admin
{
    public class SkillCategoryFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public int DisplayOrder { get; set; }
    }
}
