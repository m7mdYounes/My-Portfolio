using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.ViewModels.Admin
{
    public class ContactLinkFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Type { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Label { get; set; } = null!;

        [MaxLength(300)]
        public string? Value { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Url { get; set; } = null!;

        [MaxLength(100)]
        public string? Icon { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsPublished { get; set; } = true;
    }
}
