using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Models.Content;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.ViewModels.Admin;

namespace MyPortfolio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("admin/experiences")]
    public class ExperiencesController : Controller
    {
        private readonly IExperienceService _experienceService;

        public ExperiencesController(IExperienceService experienceService)
        {
            _experienceService = experienceService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var experiences = await _experienceService.GetAllForAdminAsync(cancellationToken);

            return View(experiences);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new ExperienceFormViewModel
            {
                StartDate = DateTime.UtcNow,
                IsPublished = true,
                Responsibilities = new List<string> { string.Empty }
            });
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ExperienceFormViewModel model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            var experience = new Experience
            {
                CompanyName = model.CompanyName,
                JobTitle = model.JobTitle,
                EmploymentType = model.EmploymentType,
                Location = model.Location,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                IsCurrent = model.IsCurrent,
                IsPublished = model.IsPublished,
                DisplayOrder = model.DisplayOrder
            };

            var result = await _experienceService.CreateAsync(
                experience,
                model.CompanyLogo,
                model.Responsibilities,
                cancellationToken);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(
            int id,
            CancellationToken cancellationToken)
        {
            var experience = await _experienceService.GetByIdAsync(id, asNoTracking: true, cancellationToken);

            if (experience is null)
                return NotFound();

            var model = new ExperienceFormViewModel
            {
                Id = experience.Id,
                CompanyName = experience.CompanyName,
                JobTitle = experience.JobTitle,
                EmploymentType = experience.EmploymentType,
                Location = experience.Location,
                CurrentCompanyLogoPath = experience.CompanyLogoPath,
                StartDate = experience.StartDate,
                EndDate = experience.EndDate,
                IsCurrent = experience.IsCurrent,
                IsPublished = experience.IsPublished,
                DisplayOrder = experience.DisplayOrder,
                Responsibilities = experience.Responsibilities
                    .OrderBy(x => x.DisplayOrder)
                    .Select(x => x.Text)
                    .ToList()
            };

            return View(model);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            ExperienceFormViewModel model,
            CancellationToken cancellationToken)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var experience = new Experience
            {
                Id = model.Id,
                CompanyName = model.CompanyName,
                JobTitle = model.JobTitle,
                EmploymentType = model.EmploymentType,
                Location = model.Location,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                IsCurrent = model.IsCurrent,
                IsPublished = model.IsPublished,
                DisplayOrder = model.DisplayOrder
            };

            var result = await _experienceService.UpdateAsync(
                experience,
                model.CompanyLogo,
                model.Responsibilities,
                cancellationToken);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _experienceService.DeleteAsync(id, cancellationToken);

            TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] =
                result.Succeeded ? result.Message : string.Join(", ", result.Errors);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("toggle-publish/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePublish(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _experienceService.TogglePublishAsync(id, cancellationToken);

            TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] =
                result.Succeeded ? result.Message : string.Join(", ", result.Errors);

            return RedirectToAction(nameof(Index));
        }
    }
}
