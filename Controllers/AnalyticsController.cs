using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.Services.Models.Analytics;
using MyPortfolio.ViewModels.Analytics;

namespace MyPortfolio.Controllers
{
    [Route("analytics")]
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpPost("track-click")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> TrackClick(
            [FromBody] TrackClickViewModel model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var visitorKey = Request.Cookies["PortfolioVisitorId"];
            var sessionKey = Request.Cookies["PortfolioSessionId"];

            if (string.IsNullOrWhiteSpace(visitorKey) || string.IsNullOrWhiteSpace(sessionKey))
                return Ok();

            var request = new TrackClickRequest
            {
                EventType = model.EventType,
                ComponentName = model.ComponentName,
                TargetType = model.TargetType,
                TargetId = model.TargetId,
                TargetText = model.TargetText,
                PagePath = model.PagePath,
                MetadataJson = model.MetadataJson
            };

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers.UserAgent.ToString();

            await _analyticsService.TrackClickAsync(
                visitorKey,
                sessionKey,
                request,
                ipAddress,
                userAgent,
                cancellationToken);

            return Ok();
        }
    }
}
