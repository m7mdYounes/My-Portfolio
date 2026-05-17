namespace MyPortfolio.Models.Content
{
    public class SkillCategory
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public int DisplayOrder { get; set; }

        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    }
}
