using MyPortfolio.Models.Content;
using MyPortfolio.Services.Models;

namespace MyPortfolio.Services.Interfaces
{
    public interface ISiteSettingService
    {
        Task<SiteSetting?> GetSettingsAsync(CancellationToken cancellationToken = default);

        Task<ServiceResult<SiteSetting>> UpdateAsync(
            SiteSetting updatedSettings,
            IFormFile? logoFile,
            IFormFile? faviconFile,
            CancellationToken cancellationToken = default);
    }
}
