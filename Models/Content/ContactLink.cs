namespace MyPortfolio.Models.Content
{
    public class ContactLink
    {
        public int Id { get; set; }

        public string Type { get; set; } = null!;
        public string Label { get; set; } = null!;

        public string? Value { get; set; }
        public string Url { get; set; } = null!;

        public string? Icon { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; } = true;
    }
}
