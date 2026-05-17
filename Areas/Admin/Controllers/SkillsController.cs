using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyPortfolio.Models.Content;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.ViewModels.Admin;

namespace MyPortfolio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("admin/skills")]
    public class SkillsController : Controller
    {
        private readonly ISkillService _skillService;

        public SkillsController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var groupedSkills = await _skillService.GetAllGroupedSkillsForAdminAsync(cancellationToken);

            return View(groupedSkills);
        }

        [HttpGet("create-category")]
        public IActionResult CreateCategory()
        {
            return View(new SkillCategoryFormViewModel());
        }

        [HttpPost("create-category")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(
            SkillCategoryFormViewModel model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            var category = new SkillCategory
            {
                Name = model.Name,
                DisplayOrder = model.DisplayOrder
            };

            var result = await _skillService.CreateCategoryAsync(category, cancellationToken);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var model = new SkillFormViewModel
            {
                IsPublished = true,
                Categories = await GetCategorySelectListAsync(cancellationToken)
            };

            return View(model);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            SkillFormViewModel model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategorySelectListAsync(cancellationToken);
                return View(model);
            }

            var skill = new Skill
            {
                SkillCategoryId = model.SkillCategoryId,
                Name = model.Name,
                IconPath = model.IconPath,
                Level = model.Level,
                DisplayOrder = model.DisplayOrder,
                IsPublished = model.IsPublished
            };

            var result = await _skillService.CreateSkillAsync(skill, cancellationToken);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                model.Categories = await GetCategorySelectListAsync(cancellationToken);
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
            var result = await _skillService.DeleteSkillAsync(id, cancellationToken);

            TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] =
                result.Succeeded ? result.Message : string.Join(", ", result.Errors);

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<SelectListItem>> GetCategorySelectListAsync(CancellationToken cancellationToken)
        {
            var categories = await _skillService.GetAllGroupedSkillsForAdminAsync(cancellationToken);

            return categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
        }
    }
}
