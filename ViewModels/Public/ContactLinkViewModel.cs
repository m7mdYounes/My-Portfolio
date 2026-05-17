namespace MyPortfolio.ViewModels.Public
{
    public class ContactLinkViewModel
    {
        public int Id { get; set; }

        public string Type { get; set; } = null!;

        public string Label { get; set; } = null!;

        public string? Value { get; set; }

        public string Url { get; set; } = null!;

        public string? Icon { get; set; }
    }
}
