using MyPortfolio.Models.Content;
using MyPortfolio.Repositories.Interfaces;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.Services.Models;

namespace MyPortfolio.Services.Implementations
{
    public class SkillService : ISkillService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkillService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SkillCategory>> GetPublishedGroupedSkillsAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<SkillCategory>().GetAllAsync(
                include: query => query.Include(x => x.Skills.Where(s => s.IsPublished).OrderBy(s => s.DisplayOrder)),
                orderBy: query => query.OrderBy(x => x.DisplayOrder),
                asNoTracking: true,
                cancellationToken: cancellationToken);
        }

        public async Task<List<SkillCategory>> GetAllGroupedSkillsForAdminAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<SkillCategory>().GetAllAsync(
                include: query => query.Include(x => x.Skills.OrderBy(s => s.DisplayOrder)),
                orderBy: query => query.OrderBy(x => x.DisplayOrder),
                asNoTracking: true,
                cancellationToken: cancellationToken);
        }

        public async Task<List<Skill>> GetAllSkillsAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<Skill>().GetAllAsync(
                include: query => query.Include(x => x.SkillCategory),
                orderBy: query => query.OrderBy(x => x.SkillCategory.DisplayOrder).ThenBy(x => x.DisplayOrder),
                asNoTracking: true,
                cancellationToken: cancellationToken);
        }

        public async Task<ServiceResult<SkillCategory>> CreateCategoryAsync(
            SkillCategory category,
            CancellationToken cancellationToken = default)
        {
            var exists = await _unitOfWork.Repository<SkillCategory>().ExistsAsync(
                x => x.Name == category.Name,
                cancellationToken);

            if (exists)
                return ServiceResult<SkillCategory>.Failure("Skill category already exists.");

            await _unitOfWork.Repository<SkillCategory>().AddAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult<SkillCategory>.Success(category, "Skill category created successfully.");
        }

        public async Task<ServiceResult<Skill>> CreateSkillAsync(
            Skill skill,
            CancellationToken cancellationToken = default)
        {
            var exists = await _unitOfWork.Repository<Skill>().ExistsAsync(
                x => x.SkillCategoryId == skill.SkillCategoryId && x.Name == skill.Name,
                cancellationToken);

            if (exists)
                return ServiceResult<Skill>.Failure("Skill already exists in this category.");

            await _unitOfWork.Repository<Skill>().AddAsync(skill, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult<Skill>.Success(skill, "Skill created successfully.");
        }

        public async Task<ServiceResult<Skill>> UpdateSkillAsync(
            Skill updatedSkill,
            CancellationToken cancellationToken = default)
        {
            var skill = await _unitOfWork.Repository<Skill>().GetByIdAsync(updatedSkill.Id, cancellationToken);

            if (skill is null)
                return ServiceResult<Skill>.Failure("Skill was not found.");

            var duplicateExists = await _unitOfWork.Repository<Skill>().ExistsAsync(
                x => x.Id != updatedSkill.Id &&
                     x.SkillCategoryId == updatedSkill.SkillCategoryId &&
                     x.Name == updatedSkill.Name,
                cancellationToken);

            if (duplicateExists)
                return ServiceResult<Skill>.Failure("Another skill with the same name exists in this category.");

            skill.SkillCategoryId = updatedSkill.SkillCategoryId;
            skill.Name = updatedSkill.Name;
            skill.IconPath = updatedSkill.IconPath;
            skill.Level = updatedSkill.Level;
            skill.DisplayOrder = updatedSkill.DisplayOrder;
            skill.IsPublished = updatedSkill.IsPublished;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult<Skill>.Success(skill, "Skill updated successfully.");
        }

        public async Task<ServiceResult> DeleteSkillAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var deleted = await _unitOfWork.Repository<Skill>().DeleteByIdAsync(id, cancellationToken);

            if (!deleted)
                return ServiceResult.Failure("Skill was not found.");

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success("Skill deleted successfully.");
        }
    }
}
