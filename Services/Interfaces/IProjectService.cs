using MyPortfolio.Models.Content;
using MyPortfolio.Services.Models;

namespace MyPortfolio.Services.Interfaces
{
    public interface IProjectService
    {
        Task<List<Project>> GetPublishedProjectsAsync(CancellationToken cancellationToken = default);

        Task<List<Project>> GetFeaturedProjectsAsync(CancellationToken cancellationToken = default);

        Task<Project?> GetPublishedProjectBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default);

        Task<Project?> GetByIdAsync(
            int id,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default);

        Task<PagedResult<Project>> GetPagedForAdminAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            CancellationToken cancellationToken = default);

        Task<ServiceResult<Project>> CreateAsync(
            Project project,
            IFormFile? mainImage,
            List<int>? skillIds,
            CancellationToken cancellationToken = default);

        Task<ServiceResult<Project>> UpdateAsync(
            Project updatedProject,
            IFormFile? mainImage,
            List<int>? skillIds,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> TogglePublishAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> ToggleFeaturedAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
