using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Data;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.Services.Models.Analytics;

namespace MyPortfolio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("admin/analytics")]
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsReportService _analyticsReportService;

        public AnalyticsController(IAnalyticsReportService analyticsReportService)
        {
            _analyticsReportService = analyticsReportService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(
            DateTime? fromDate,
            DateTime? toDate,
            CancellationToken cancellationToken)
        {
            var viewModel = await _analyticsReportService.GetDashboardAsync(
                fromDate,
                toDate,
                cancellationToken);

            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(viewModel);
        }
    }
}
