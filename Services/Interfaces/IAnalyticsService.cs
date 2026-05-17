using MyPortfolio.Services.Models;
using MyPortfolio.Services.Models.Analytics;

namespace MyPortfolio.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task TrackPageViewAsync(
            string visitorKey,
            string sessionKey,
            string path,
            string? fullUrl,
            string? pageTitle,
            string? referrer,
            string? ipAddress,
            string? userAgent,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> TrackClickAsync(
            string visitorKey,
            string sessionKey,
            TrackClickRequest request,
            string? ipAddress,
            string? userAgent,
            CancellationToken cancellationToken = default);
    }
}
