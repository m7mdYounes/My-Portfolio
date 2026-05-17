using Microsoft.EntityFrameworkCore;
using MyPortfolio.Helpers.Constants;
using MyPortfolio.Helpers.Interfaces;
using MyPortfolio.Models.Content;
using MyPortfolio.Repositories.Interfaces;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.Services.Models;
using System.Linq.Expressions;

namespace MyPortfolio.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageHelper _fileStorageHelper;
        private readonly ISlugHelper _slugHelper;

        public ProjectService(
            IUnitOfWork unitOfWork,
            IFileStorageHelper fileStorageHelper,
            ISlugHelper slugHelper)
        {
            _unitOfWork = unitOfWork;
            _fileStorageHelper = fileStorageHelper;
            _slugHelper = slugHelper;
        }

        public async Task<List<Project>> GetPublishedProjectsAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<Project>().GetAllAsync(
                predicate: x => x.IsPublished,
                include: query => query
                    .Include(x => x.ProjectSkills)
                    .ThenInclude(x => x.Skill),
                orderBy: query => query.OrderBy(x => x.DisplayOrder),
                asNoTracking: true,
                cancellationToken: cancellationToken);
        }

        public async Task<List<Project>> GetFeaturedProjectsAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<Project>().GetAllAsync(
                predicate: x => x.IsPublished && x.IsFeatured,
                include: query => query
                    .Include(x => x.ProjectSkills)
                    .ThenInclude(x => x.Skill),
                orderBy: query => query.OrderBy(x => x.DisplayOrder),
                asNoTracking: true,
                cancellationToken: cancellationToken);
        }

        public async Task<Project?> GetPublishedProjectBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<Project>().FirstOrDefaultAsync(
                predicate: x => x.Slug == slug && x.IsPublished,
                include: query => query
                    .Include(x => x.Images.OrderBy(i => i.DisplayOrder))
                    .Include(x => x.ProjectSkills)
                    .ThenInclude(x => x.Skill),
                asNoTracking: true,
                cancellationToken: cancellationToken);
        }

        public async Task<Project?> GetByIdAsync(
            int id,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<Project>().FirstOrDefaultAsync(
                predicate: x => x.Id == id,
                include: query => query
                    .Include(x => x.Images.OrderBy(i => i.DisplayOrder))
                    .Include(x => x.ProjectSkills)
                    .ThenInclude(x => x.Skill),
                asNoTracking: asNoTracking,
                cancellationToken: cancellationToken);
        }

        public async Task<PagedResult<Project>> GetPagedForAdminAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            CancellationToken cancellationToken = default)
        {
            Expression<Func<Project, bool>>? predicate = null;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim();

                predicate = x =>
                    x.Title.Contains(term) ||
                    x.ShortDescription.Contains(term) ||
                    x.Slug.Contains(term);
            }

            var result = await _unitOfWork.Repository<Project>().GetPagedAsync(
                pageNumber: pageNumber,
                pageSize: pageSize,
                predicate: predicate,
                include: query => query
                    .Include(x => x.ProjectSkills)
                    .ThenInclude(x => x.Skill),
                orderBy: query => query.OrderBy(x => x.DisplayOrder).ThenByDescending(x => x.CreatedAt),
                asNoTracking: true,
                cancellationToken: cancellationToken);

            return new PagedResult<Project>
            {
                Items = result.Items,
                TotalCount = result.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ServiceResult<Project>> CreateAsync(
            Project project,
            IFormFile? mainImage,
            List<int>? skillIds,
            CancellationToken cancellationToken = default)
        {
            project.Slug = await GenerateUniqueSlugAsync(project.Title, null, cancellationToken);

            if (mainImage is not null)
            {
                var uploadResult = await _fileStorageHelper.SaveImageAsync(
                    mainImage,
                    UploadFolders.Projects,
                    maxFileSizeInBytes: 3 * 1024 * 1024,
                    cancellationToken: cancellationToken);

                if (!uploadResult.Succeeded)
                    return ServiceResult<Project>.Failure(uploadResult.ErrorMessage ?? "Failed to upload project image.");

                project.MainImagePath = uploadResult.RelativePath;
            }

            project.CreatedAt = DateTime.UtcNow;
            project.UpdatedAt = null;

            if (skillIds is not null && skillIds.Any())
            {
                foreach (var skillId in skillIds.Distinct())
                {
                    project.ProjectSkills.Add(new ProjectSkill
                    {
                        SkillId = skillId,
                        Project = project
                    });
                }
            }

            await _unitOfWork.Repository<Project>().AddAsync(project, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult<Project>.Success(project, "Project created successfully.");
        }

        public async Task<ServiceResult<Project>> UpdateAsync(
            Project updatedProject,
            IFormFile? mainImage,
            List<int>? skillIds,
            CancellationToken cancellationToken = default)
        {
            var project = await GetByIdAsync(updatedProject.Id, asNoTracking: false, cancellationToken);

            if (project is null)
                return ServiceResult<Project>.Failure("Project was not found.");

            var oldImagePath = project.MainImagePath;

            if (!string.Equals(project.Title, updatedProject.Title, StringComparison.OrdinalIgnoreCase))
            {
                project.Slug = await GenerateUniqueSlugAsync(updatedProject.Title, project.Id, cancellationToken);
            }

            if (mainImage is not null)
            {
                var uploadResult = await _fileStorageHelper.SaveImageAsync(
                    mainImage,
                    UploadFolders.Projects,
                    maxFileSizeInBytes: 3 * 1024 * 1024,
                    cancellationToken: cancellationToken);

                if (!uploadResult.Succeeded)
                    return ServiceResult<Project>.Failure(uploadResult.ErrorMessage ?? "Failed to upload project image.");

                project.MainImagePath = uploadResult.RelativePath;
            }

            project.Title = updatedProject.Title;
            project.ShortDescription = updatedProject.ShortDescription;
            project.FullDescription = updatedProject.FullDescription;
            project.ProblemStatement = updatedProject.ProblemStatement;
            project.SolutionOverview = updatedProject.SolutionOverview;
            project.TechnicalHighlights = updatedProject.TechnicalHighlights;
            project.DemoUrl = updatedProject.DemoUrl;
            project.GitHubUrl = updatedProject.GitHubUrl;
            project.IsFeatured = updatedProject.IsFeatured;
            project.IsPublished = updatedProject.IsPublished;
            project.DisplayOrder = updatedProject.DisplayOrder;
            project.StartDate = updatedProject.StartDate;
            project.EndDate = updatedProject.EndDate;
            project.UpdatedAt = DateTime.UtcNow;

            SyncProjectSkills(project, skillIds);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (mainImage is not null)
                _fileStorageHelper.DeleteFile(oldImagePath);

            return ServiceResult<Project>.Success(project, "Project updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var project = await GetByIdAsync(id, asNoTracking: false, cancellationToken);

            if (project is null)
                return ServiceResult.Failure("Project was not found.");

            var imagePaths = project.Images.Select(x => x.ImagePath).ToList();

            if (!string.IsNullOrWhiteSpace(project.MainImagePath))
                imagePaths.Add(project.MainImagePath);

            _unitOfWork.Repository<Project>().Delete(project);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            foreach (var imagePath in imagePaths)
                _fileStorageHelper.DeleteFile(imagePath);

            return ServiceResult.Success("Project deleted successfully.");
        }

        public async Task<ServiceResult> TogglePublishAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(id, cancellationToken);

            if (project is null)
                return ServiceResult.Failure("Project was not found.");

            project.IsPublished = !project.IsPublished;
            project.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success("Project publish status updated successfully.");
        }

        public async Task<ServiceResult> ToggleFeaturedAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(id, cancellationToken);

            if (project is null)
                return ServiceResult.Failure("Project was not found.");

            project.IsFeatured = !project.IsFeatured;
            project.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success("Project featured status updated successfully.");
        }

        private async Task<string> GenerateUniqueSlugAsync(
            string title,
            int? currentProjectId,
            CancellationToken cancellationToken)
        {
            var baseSlug = _slugHelper.GenerateSlug(title);

            if (string.IsNullOrWhiteSpace(baseSlug))
                baseSlug = Guid.NewGuid().ToString("N")[..12];

            var slug = baseSlug;
            var counter = 2;

            while (await _unitOfWork.Repository<Project>().ExistsAsync(
                x => x.Slug == slug && (!currentProjectId.HasValue || x.Id != currentProjectId.Value),
                cancellationToken))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }

        private static void SyncProjectSkills(Project project, List<int>? skillIds)
        {
            skillIds ??= new List<int>();

            var newSkillIds = skillIds.Distinct().ToList();

            var existingSkillIds = project.ProjectSkills
                .Select(x => x.SkillId)
                .ToList();

            var toRemove = project.ProjectSkills
                .Where(x => !newSkillIds.Contains(x.SkillId))
                .ToList();

            foreach (var item in toRemove)
                project.ProjectSkills.Remove(item);

            var toAdd = newSkillIds
                .Where(skillId => !existingSkillIds.Contains(skillId))
                .ToList();

            foreach (var skillId in toAdd)
            {
                project.ProjectSkills.Add(new ProjectSkill
                {
                    ProjectId = project.Id,
                    SkillId = skillId
                });
            }
        }
    }
}
