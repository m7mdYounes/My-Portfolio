using MyPortfolio.Models.Content;
using MyPortfolio.Services.Models;

namespace MyPortfolio.Services.Interfaces
{
    public interface IContactLinkService
    {
        Task<List<ContactLink>> GetPublishedAsync(CancellationToken cancellationToken = default);

        Task<List<ContactLink>> GetAllForAdminAsync(CancellationToken cancellationToken = default);

        Task<ContactLink?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<ServiceResult<ContactLink>> CreateAsync(
            ContactLink contactLink,
            CancellationToken cancellationToken = default);

        Task<ServiceResult<ContactLink>> UpdateAsync(
            ContactLink contactLink,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
