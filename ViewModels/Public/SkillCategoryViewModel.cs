namespace MyPortfolio.ViewModels.Public
{
    public class SkillCategoryViewModel
    {
        public string Name { get; set; } = null!;

        public List<SkillViewModel> Skills { get; set; } = new();
    }

    public class SkillViewModel
    {
        public string Name { get; set; } = null!;

        public string? IconPath { get; set; }

        public string? Level { get; set; }
    }
}
