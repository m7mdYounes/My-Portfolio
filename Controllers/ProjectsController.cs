using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.ViewModels.Public;

namespace MyPortfolio.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var projects = await _projectService.GetPublishedProjectsAsync(cancellationToken);

            var viewModel = projects.Select(x => new ProjectCardViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                ShortDescription = x.ShortDescription,
                MainImagePath = x.MainImagePath,
                IsFeatured = x.IsFeatured,
                Skills = x.ProjectSkills
                    .Select(ps => ps.Skill.Name)
                    .ToList()
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(
            string slug,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return NotFound();

            var project = await _projectService.GetPublishedProjectBySlugAsync(slug, cancellationToken);

            if (project is null)
                return NotFound();

            var viewModel = new ProjectDetailsViewModel
            {
                Id = project.Id,
                Title = project.Title,
                Slug = project.Slug,
                ShortDescription = project.ShortDescription,
                FullDescription = project.FullDescription,
                ProblemStatement = project.ProblemStatement,
                SolutionOverview = project.SolutionOverview,
                TechnicalHighlights = project.TechnicalHighlights,
                MainImagePath = project.MainImagePath,
                DemoUrl = project.DemoUrl,
                GitHubUrl = project.GitHubUrl,
                Images = project.Images
                    .OrderBy(x => x.DisplayOrder)
                    .Select(x => x.ImagePath)
                    .ToList(),
                Skills = project.ProjectSkills
                    .Select(x => x.Skill.Name)
                    .ToList()
            };

            return View(viewModel);
        }
    }
}
