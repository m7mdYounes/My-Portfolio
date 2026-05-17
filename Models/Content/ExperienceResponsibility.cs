namespace MyPortfolio.Models.Content
{
    public class ExperienceResponsibility
    {
        public int Id { get; set; }

        public int ExperienceId { get; set; }
        public Experience Experience { get; set; } = null!;

        public string Text { get; set; } = null!;
        public int DisplayOrder { get; set; }
    }
}
