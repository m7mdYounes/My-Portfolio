global using Microsoft.EntityFrameworkCore;
using MyPortfolio.Helpers.Constants;
using MyPortfolio.Helpers.Interfaces;
using MyPortfolio.Models.Content;
using MyPortfolio.Repositories.Interfaces;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.Services.Models;

namespace MyPortfolio.Services.Implementations
{
    public class ExperienceService : IExperienceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageHelper _fileStorageHelper;

        public ExperienceService(
            IUnitOfWork unitOfWork,
            IFileStorageHelper fileStorageHelper)
        {
            _unitOfWork = unitOfWork;
            _fileStorageHelper = fileStorageHelper;
        }

        public async Task<List<Experience>> GetPublishedExperiencesAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<Experience>().GetAllAsync(
                predicate: x => x.IsPublished,
                include: query => query.Include(x => x.Responsibilities.OrderBy(r => r.DisplayOrder)),
                orderBy: query => query.OrderBy(x => x.DisplayOrder).ThenByDescending(x => x.StartDate),
                asNoTracking: true,
                cancellationToken: cancellationToken);
        }

        public async Task<List<Experience>> GetAllForAdminAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<Experience>().GetAllAsync(
                include: query => query.Include(x => x.Responsibilities.OrderBy(r => r.DisplayOrder)),
                orderBy: query => query.OrderBy(x => x.DisplayOrder).ThenByDescending(x => x.StartDate),
                asNoTracking: true,
                cancellationToken: cancellationToken);
        }

        public async Task<Experience?> GetByIdAsync(
            int id,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<Experience>().FirstOrDefaultAsync(
                predicate: x => x.Id == id,
                include: query => query.Include(x => x.Responsibilities.OrderBy(r => r.DisplayOrder)),
                asNoTracking: asNoTracking,
                cancellationToken: cancellationToken);
        }

        public async Task<ServiceResult<Experience>> CreateAsync(
            Experience experience,
            IFormFile? companyLogo,
            List<string>? responsibilities,
            CancellationToken cancellationToken = default)
        {
            if (companyLogo is not null)
            {
                var uploadResult = await _fileStorageHelper.SaveImageAsync(
                    companyLogo,
                    UploadFolders.Companies,
                    cancellationToken: cancellationToken);

                if (!uploadResult.Succeeded)
                    return ServiceResult<Experience>.Failure(uploadResult.ErrorMessage ?? "Failed to upload company logo.");

                experience.CompanyLogoPath = uploadResult.RelativePath;
            }

            AddResponsibilities(experience, responsibilities);

            await _unitOfWork.Repository<Experience>().AddAsync(experience, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult<Experience>.Success(experience, "Experience created successfully.");
        }

        public async Task<ServiceResult<Experience>> UpdateAsync(
            Experience updatedExperience,
            IFormFile? companyLogo,
            List<string>? responsibilities,
            CancellationToken cancellationToken = default)
        {
            var experience = await GetByIdAsync(updatedExperience.Id, asNoTracking: false, cancellationToken);

            if (experience is null)
                return ServiceResult<Experience>.Failure("Experience was not found.");

            var oldLogoPath = experience.CompanyLogoPath;

            if (companyLogo is not null)
            {
                var uploadResult = await _fileStorageHelper.SaveImageAsync(
                    companyLogo,
                    UploadFolders.Companies,
                    cancellationToken: cancellationToken);

                if (!uploadResult.Succeeded)
                    return ServiceResult<Experience>.Failure(uploadResult.ErrorMessage ?? "Failed to upload company logo.");

                experience.CompanyLogoPath = uploadResult.RelativePath;
            }

            experience.CompanyName = updatedExperience.CompanyName;
            experience.JobTitle = updatedExperience.JobTitle;
            experience.EmploymentType = updatedExperience.EmploymentType;
            experience.Location = updatedExperience.Location;
            experience.StartDate = updatedExperience.StartDate;
            experience.EndDate = updatedExperience.EndDate;
            experience.IsCurrent = updatedExperience.IsCurrent;
            experience.IsPublished = updatedExperience.IsPublished;
            experience.DisplayOrder = updatedExperience.DisplayOrder;

            experience.Responsibilities.Clear();
            AddResponsibilities(experience, responsibilities);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (companyLogo is not null)
                _fileStorageHelper.DeleteFile(oldLogoPath);

            return ServiceResult<Experience>.Success(experience, "Experience updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var experience = await GetByIdAsync(id, asNoTracking: false, cancellationToken);

            if (experience is null)
                return ServiceResult.Failure("Experience was not found.");

            var oldLogoPath = experience.CompanyLogoPath;

            _unitOfWork.Repository<Experience>().Delete(experience);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _fileStorageHelper.DeleteFile(oldLogoPath);

            return ServiceResult.Success("Experience deleted successfully.");
        }

        public async Task<ServiceResult> TogglePublishAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var experience = await _unitOfWork.Repository<Experience>().GetByIdAsync(id, cancellationToken);

            if (experience is null)
                return ServiceResult.Failure("Experience was not found.");

            experience.IsPublished = !experience.IsPublished;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success("Experience publish status updated successfully.");
        }

        private static void AddResponsibilities(Experience experience, List<string>? responsibilities)
        {
            if (responsibilities is null)
                return;

            var cleanResponsibilities = responsibilities
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .ToList();

            for (var i = 0; i < cleanResponsibilities.Count; i++)
            {
                experience.Responsibilities.Add(new ExperienceResponsibility
                {
                    Text = cleanResponsibilities[i],
                    DisplayOrder = i + 1
                });
            }
        }
    }
}
