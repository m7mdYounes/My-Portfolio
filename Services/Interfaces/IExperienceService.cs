using MyPortfolio.Models.Content;
using MyPortfolio.Services.Models;

namespace MyPortfolio.Services.Interfaces
{
    public interface IExperienceService
    {
        Task<List<Experience>> GetPublishedExperiencesAsync(CancellationToken cancellationToken = default);

        Task<List<Experience>> GetAllForAdminAsync(CancellationToken cancellationToken = default);

        Task<Experience?> GetByIdAsync(
            int id,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default);

        Task<ServiceResult<Experience>> CreateAsync(
            Experience experience,
            IFormFile? companyLogo,
            List<string>? responsibilities,
            CancellationToken cancellationToken = default);

        Task<ServiceResult<Experience>> UpdateAsync(
            Experience updatedExperience,
            IFormFile? companyLogo,
            List<string>? responsibilities,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> TogglePublishAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
