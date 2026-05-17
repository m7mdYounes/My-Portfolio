using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.ViewModels.Admin
{
    public class SiteSettingsEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string SiteTitle { get; set; } = null!;

        [MaxLength(500)]
        public string? MetaDescription { get; set; }

        public string? CurrentLogoPath { get; set; }

        public IFormFile? LogoFile { get; set; }

        public string? CurrentFaviconPath { get; set; }

        public IFormFile? FaviconFile { get; set; }

        [MaxLength(20)]
        public string? PrimaryColor { get; set; }

        [MaxLength(20)]
        public string? SecondaryColor { get; set; }

        public bool EnableDarkMode { get; set; }
    }
}
