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
    [Route("admin/projects")]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ISkillService _skillService;

        public ProjectsController(
            IProjectService projectService,
            ISkillService skillService)
        {
            _projectService = projectService;
            _skillService = skillService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            CancellationToken cancellationToken = default)
        {
            var projects = await _projectService.GetPagedForAdminAsync(
                pageNumber,
                pageSize,
                searchTerm,
                cancellationToken);

            return View(projects);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var viewModel = new ProjectFormViewModel
            {
                IsPublished = true,
                AvailableSkills = await GetSkillSelectListAsync(cancellationToken)
            };

            return View(viewModel);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ProjectFormViewModel model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableSkills = await GetSkillSelectListAsync(cancellationToken);
                return View(model);
            }

            var project = new Project
            {
                Title = model.Title,
                ShortDescription = model.ShortDescription,
                FullDescription = model.FullDescription,
                ProblemStatement = model.ProblemStatement,
                SolutionOverview = model.SolutionOverview,
                TechnicalHighlights = model.TechnicalHighlights,
                DemoUrl = model.DemoUrl,
                GitHubUrl = model.GitHubUrl,
                IsFeatured = model.IsFeatured,
                IsPublished = model.IsPublished,
                DisplayOrder = model.DisplayOrder,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            var result = await _projectService.CreateAsync(
                project,
                model.MainImage,
                model.SelectedSkillIds,
                cancellationToken);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                model.AvailableSkills = await GetSkillSelectListAsync(cancellationToken);
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
            var project = await _projectService.GetByIdAsync(id, asNoTracking: true, cancellationToken);

            if (project is null)
                return NotFound();

            var viewModel = new ProjectFormViewModel
            {
                Id = project.Id,
                Title = project.Title,
                Slug = project.Slug,
                ShortDescription = project.ShortDescription,
                FullDescription = project.FullDescription,
                ProblemStatement = project.ProblemStatement,
                SolutionOverview = project.SolutionOverview,
                TechnicalHighlights = project.TechnicalHighlights,
                CurrentMainImagePath = project.MainImagePath,
                DemoUrl = project.DemoUrl,
                GitHubUrl = project.GitHubUrl,
                IsFeatured = project.IsFeatured,
                IsPublished = project.IsPublished,
                DisplayOrder = project.DisplayOrder,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                SelectedSkillIds = project.ProjectSkills.Select(x => x.SkillId).ToList(),
                AvailableSkills = await GetSkillSelectListAsync(cancellationToken)
            };

            return View(viewModel);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            ProjectFormViewModel model,
            CancellationToken cancellationToken)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                model.AvailableSkills = await GetSkillSelectListAsync(cancellationToken);
                return View(model);
            }

            var project = new Project
            {
                Id = model.Id,
                Title = model.Title,
                ShortDescription = model.ShortDescription,
                FullDescription = model.FullDescription,
                ProblemStatement = model.ProblemStatement,
                SolutionOverview = model.SolutionOverview,
                TechnicalHighlights = model.TechnicalHighlights,
                DemoUrl = model.DemoUrl,
                GitHubUrl = model.GitHubUrl,
                IsFeatured = model.IsFeatured,
                IsPublished = model.IsPublished,
                DisplayOrder = model.DisplayOrder,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            var result = await _projectService.UpdateAsync(
                project,
                model.MainImage,
                model.SelectedSkillIds,
                cancellationToken);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                model.AvailableSkills = await GetSkillSelectListAsync(cancellationToken);
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
            var result = await _projectService.DeleteAsync(id, cancellationToken);

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
            var result = await _projectService.TogglePublishAsync(id, cancellationToken);

            TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] =
                result.Succeeded ? result.Message : string.Join(", ", result.Errors);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("toggle-featured/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFeatured(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _projectService.ToggleFeaturedAsync(id, cancellationToken);

            TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] =
                result.Succeeded ? result.Message : string.Join(", ", result.Errors);

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<SelectListItem>> GetSkillSelectListAsync(CancellationToken cancellationToken)
        {
            var skills = await _skillService.GetAllSkillsAsync(cancellationToken);

            return skills.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.SkillCategory.Name} - {x.Name}"
            }).ToList();
        }
    }
}
