using MyPortfolio.Helpers.Constants;
using MyPortfolio.Helpers.Interfaces;
using MyPortfolio.Models.Content;
using MyPortfolio.Repositories.Interfaces;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.Services.Models;

namespace MyPortfolio.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageHelper _fileStorageHelper;

        public ProfileService(
            IUnitOfWork unitOfWork,
            IFileStorageHelper fileStorageHelper)
        {
            _unitOfWork = unitOfWork;
            _fileStorageHelper = fileStorageHelper;
        }

        public async Task<Profile?> GetProfileAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<Profile>().FirstOrDefaultAsync(
                predicate: x => x.Id == 1,
                asNoTracking: true,
                cancellationToken: cancellationToken);
        }

        public async Task<ServiceResult<Profile>> UpdateProfileAsync(
            Profile updatedProfile,
            IFormFile? profileImage,
            IFormFile? cvFile,
            CancellationToken cancellationToken = default)
        {
            var profile = await _unitOfWork.Repository<Profile>().GetByIdAsync(1, cancellationToken);

            if (profile is null)
                return ServiceResult<Profile>.Failure("Profile was not found.");

            var oldImagePath = profile.ProfileImagePath;
            var oldCvPath = profile.CvFilePath;

            if (profileImage is not null)
            {
                var imageResult = await _fileStorageHelper.SaveImageAsync(
                    profileImage,
                    UploadFolders.Profile,
                    cancellationToken: cancellationToken);

                if (!imageResult.Succeeded)
                    return ServiceResult<Profile>.Failure(imageResult.ErrorMessage ?? "Failed to upload profile image.");

                profile.ProfileImagePath = imageResult.RelativePath;
            }

            if (cvFile is not null)
            {
                var cvResult = await _fileStorageHelper.SavePdfAsync(
                    cvFile,
                    UploadFolders.Cv,
                    cancellationToken: cancellationToken);

                if (!cvResult.Succeeded)
                    return ServiceResult<Profile>.Failure(cvResult.ErrorMessage ?? "Failed to upload CV.");

                profile.CvFilePath = cvResult.RelativePath;
            }

            profile.FullName = updatedProfile.FullName;
            profile.Title = updatedProfile.Title;
            profile.ShortSummary = updatedProfile.ShortSummary;
            profile.LongSummary = updatedProfile.LongSummary;
            profile.Location = updatedProfile.Location;
            profile.YearsOfExperience = updatedProfile.YearsOfExperience;
            profile.IsAvailableForWork = updatedProfile.IsAvailableForWork;
            profile.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (profileImage is not null)
                _fileStorageHelper.DeleteFile(oldImagePath);

            if (cvFile is not null)
                _fileStorageHelper.DeleteFile(oldCvPath);

            return ServiceResult<Profile>.Success(profile, "Profile updated successfully.");
        }
    }
}
