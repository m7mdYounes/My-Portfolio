using MyPortfolio.Models.Content;
using MyPortfolio.Services.Models;

namespace MyPortfolio.Services.Interfaces
{
    public interface IProfileService
    {
        Task<Profile?> GetProfileAsync(CancellationToken cancellationToken = default);

        Task<ServiceResult<Profile>> UpdateProfileAsync(
            Profile profile,
            IFormFile? profileImage,
            IFormFile? cvFile,
            CancellationToken cancellationToken = default);
    }
}
