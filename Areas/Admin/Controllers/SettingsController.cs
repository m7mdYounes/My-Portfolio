using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Models.Content;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.ViewModels.Admin;

namespace MyPortfolio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("admin/settings")]
    public class SettingsController : Controller
    {
        private readonly ISiteSettingService _siteSettingService;

        public SettingsController(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Edit(CancellationToken cancellationToken)
        {
            var settings = await _siteSettingService.GetSettingsAsync(cancellationToken);

            if (settings is null)
                return NotFound();

            var model = new SiteSettingsEditViewModel
            {
                Id = settings.Id,
                SiteTitle = settings.SiteTitle,
                MetaDescription = settings.MetaDescription,
                CurrentLogoPath = settings.LogoPath,
                CurrentFaviconPath = settings.FaviconPath,
                PrimaryColor = settings.PrimaryColor,
                SecondaryColor = settings.SecondaryColor,
                EnableDarkMode = settings.EnableDarkMode
            };

            return View(model);
        }

        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            SiteSettingsEditViewModel model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            var settings = new SiteSetting
            {
                Id = model.Id,
                SiteTitle = model.SiteTitle,
                MetaDescription = model.MetaDescription,
                PrimaryColor = model.PrimaryColor,
                SecondaryColor = model.SecondaryColor,
                EnableDarkMode = model.EnableDarkMode
            };

            var result = await _siteSettingService.UpdateAsync(
                settings,
                model.LogoFile,
                model.FaviconFile,
                cancellationToken);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Edit));
        }
    }
}
