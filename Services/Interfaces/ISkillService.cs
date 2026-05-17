using MyPortfolio.Models.Content;
using MyPortfolio.Services.Models;

namespace MyPortfolio.Services.Interfaces
{
    public interface ISkillService
    {
        Task<List<SkillCategory>> GetPublishedGroupedSkillsAsync(CancellationToken cancellationToken = default);

        Task<List<SkillCategory>> GetAllGroupedSkillsForAdminAsync(CancellationToken cancellationToken = default);

        Task<List<Skill>> GetAllSkillsAsync(CancellationToken cancellationToken = default);

        Task<ServiceResult<SkillCategory>> CreateCategoryAsync(
            SkillCategory category,
            CancellationToken cancellationToken = default);

        Task<ServiceResult<Skill>> CreateSkillAsync(
            Skill skill,
            CancellationToken cancellationToken = default);

        Task<ServiceResult<Skill>> UpdateSkillAsync(
            Skill skill,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> DeleteSkillAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
