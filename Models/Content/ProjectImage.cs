namespace MyPortfolio.Models.Content
{
    public class ProjectImage
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public string ImagePath { get; set; } = null!;
        public string? Caption { get; set; }

        public int DisplayOrder { get; set; }
    }
}
