namespace MyPortfolio.Models.Content
{
    public class SiteSetting
    {
        public int Id { get; set; }

        public string SiteTitle { get; set; } = null!;
        public string? MetaDescription { get; set; }

        public string? LogoPath { get; set; }
        public string? FaviconPath { get; set; }

        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }

        public bool EnableDarkMode { get; set; } = true;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
