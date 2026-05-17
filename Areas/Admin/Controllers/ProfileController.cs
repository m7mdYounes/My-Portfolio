using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Models.Content;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.ViewModels.Admin;

namespace MyPortfolio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("admin/profile")]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Edit(CancellationToken cancellationToken)
        {
            var profile = await _profileService.GetProfileAsync(cancellationToken);

            if (profile is null)
                return NotFound();

            var viewModel = new ProfileEditViewModel
            {
                Id = profile.Id,
                FullName = profile.FullName,
                Title = profile.Title,
                ShortSummary = profile.ShortSummary,
                LongSummary = profile.LongSummary,
                CurrentProfileImagePath = profile.ProfileImagePath,
                CurrentCvFilePath = profile.CvFilePath,
                Location = profile.Location,
                YearsOfExperience = profile.YearsOfExperience,
                IsAvailableForWork = profile.IsAvailableForWork
            };

            return View(viewModel);
        }

        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            ProfileEditViewModel model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            var profile = new Profile
            {
                Id = model.Id,
                FullName = model.FullName,
                Title = model.Title,
                ShortSummary = model.ShortSummary,
                LongSummary = model.LongSummary,
                Location = model.Location,
                YearsOfExperience = model.YearsOfExperience,
                IsAvailableForWork = model.IsAvailableForWork
            };

            var result = await _profileService.UpdateProfileAsync(
                profile,
                model.ProfileImage,
                model.CvFile,
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
