using MyPortfolio.Helpers.Constants;
using MyPortfolio.Helpers.Interfaces;
using MyPortfolio.Models.Content;
using MyPortfolio.Repositories.Interfaces;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.Services.Models;

namespace MyPortfolio.Services.Implementations
{
    public class SiteSettingService : ISiteSettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageHelper _fileStorageHelper;

        public SiteSettingService(
            IUnitOfWork unitOfWork,
            IFileStorageHelper fileStorageHelper)
        {
            _unitOfWork = unitOfWork;
            _fileStorageHelper = fileStorageHelper;
        }

        public async Task<SiteSetting?> GetSettingsAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<SiteSetting>().FirstOrDefaultAsync(
                predicate: x => x.Id == 1,
                asNoTracking: true,
                cancellationToken: cancellationToken);
        }

        public async Task<ServiceResult<SiteSetting>> UpdateAsync(
            SiteSetting updatedSettings,
            IFormFile? logoFile,
            IFormFile? faviconFile,
            CancellationToken cancellationToken = default)
        {
            var settings = await _unitOfWork.Repository<SiteSetting>().GetByIdAsync(1, cancellationToken);

            if (settings is null)
                return ServiceResult<SiteSetting>.Failure("Site settings were not found.");

            var oldLogoPath = settings.LogoPath;
            var oldFaviconPath = settings.FaviconPath;

            if (logoFile is not null)
            {
                var uploadResult = await _fileStorageHelper.SaveImageAsync(
                    logoFile,
                    UploadFolders.Site,
                    cancellationToken: cancellationToken);

                if (!uploadResult.Succeeded)
                    return ServiceResult<SiteSetting>.Failure(uploadResult.ErrorMessage ?? "Failed to upload logo.");

                settings.LogoPath = uploadResult.RelativePath;
            }

            if (faviconFile is not null)
            {
                var uploadResult = await _fileStorageHelper.SaveImageAsync(
                    faviconFile,
                    UploadFolders.Site,
                    maxFileSizeInBytes: 512 * 1024,
                    cancellationToken: cancellationToken);

                if (!uploadResult.Succeeded)
                    return ServiceResult<SiteSetting>.Failure(uploadResult.ErrorMessage ?? "Failed to upload favicon.");

                settings.FaviconPath = uploadResult.RelativePath;
            }

            settings.SiteTitle = updatedSettings.SiteTitle;
            settings.MetaDescription = updatedSettings.MetaDescription;
            settings.PrimaryColor = updatedSettings.PrimaryColor;
            settings.SecondaryColor = updatedSettings.SecondaryColor;
            settings.EnableDarkMode = updatedSettings.EnableDarkMode;
            settings.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (logoFile is not null)
                _fileStorageHelper.DeleteFile(oldLogoPath);

            if (faviconFile is not null)
                _fileStorageHelper.DeleteFile(oldFaviconPath);

            return ServiceResult<SiteSetting>.Success(settings, "Site settings updated successfully.");
        }
    }
}
