using MyPortfolio.Models.Content;
using MyPortfolio.Repositories.Interfaces;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.Services.Models;

namespace MyPortfolio.Services.Implementations
{
    public class ContactLinkService : IContactLinkService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactLinkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ContactLink>> GetPublishedAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<ContactLink>().GetAllAsync(
                predicate: x => x.IsPublished,
                orderBy: query => query.OrderBy(x => x.DisplayOrder),
                asNoTracking: true,
                cancellationToken: cancellationToken);
        }

        public async Task<List<ContactLink>> GetAllForAdminAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<ContactLink>().GetAllAsync(
                orderBy: query => query.OrderBy(x => x.DisplayOrder),
                asNoTracking: true,
                cancellationToken: cancellationToken);
        }

        public async Task<ContactLink?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<ContactLink>().GetByIdAsync(id, cancellationToken);
        }

        public async Task<ServiceResult<ContactLink>> CreateAsync(
            ContactLink contactLink,
            CancellationToken cancellationToken = default)
        {
            await _unitOfWork.Repository<ContactLink>().AddAsync(contactLink, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult<ContactLink>.Success(contactLink, "Contact link created successfully.");
        }

        public async Task<ServiceResult<ContactLink>> UpdateAsync(
            ContactLink updatedContactLink,
            CancellationToken cancellationToken = default)
        {
            var contactLink = await _unitOfWork.Repository<ContactLink>().GetByIdAsync(
                updatedContactLink.Id,
                cancellationToken);

            if (contactLink is null)
                return ServiceResult<ContactLink>.Failure("Contact link was not found.");

            contactLink.Type = updatedContactLink.Type;
            contactLink.Label = updatedContactLink.Label;
            contactLink.Value = updatedContactLink.Value;
            contactLink.Url = updatedContactLink.Url;
            contactLink.Icon = updatedContactLink.Icon;
            contactLink.DisplayOrder = updatedContactLink.DisplayOrder;
            contactLink.IsPublished = updatedContactLink.IsPublished;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult<ContactLink>.Success(contactLink, "Contact link updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var deleted = await _unitOfWork.Repository<ContactLink>().DeleteByIdAsync(id, cancellationToken);

            if (!deleted)
                return ServiceResult.Failure("Contact link was not found.");

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success("Contact link deleted successfully.");
        }
    }
}
