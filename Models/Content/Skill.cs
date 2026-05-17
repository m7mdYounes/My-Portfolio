namespace MyPortfolio.Models.Content
{
    public class Skill
    {
        public int Id { get; set; }

        public int SkillCategoryId { get; set; }
        public SkillCategory SkillCategory { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string? IconPath { get; set; }

        public string? Level { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; } = true;

        public ICollection<ProjectSkill> ProjectSkills { get; set; } = new List<ProjectSkill>();
    }
}
