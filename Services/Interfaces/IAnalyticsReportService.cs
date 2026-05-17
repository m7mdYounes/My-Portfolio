using MyPortfolio.Services.Models.Analytics;

namespace MyPortfolio.Services.Interfaces
{
    public interface IAnalyticsReportService
    {
        Task<AnalyticsDashboardViewModel> GetDashboardAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default);
    }
}
