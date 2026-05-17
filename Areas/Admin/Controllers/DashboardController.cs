using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Models.Content;
using MyPortfolio.Repositories.Interfaces;
using MyPortfolio.ViewModels.Admin;

namespace MyPortfolio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("admin")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("")]
        [HttpGet("dashboard")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var viewModel = new DashboardViewModel
            {
                TotalProjects = await _unitOfWork.Repository<Project>().CountAsync(null, cancellationToken),
                PublishedProjects = await _unitOfWork.Repository<Project>().CountAsync(x => x.IsPublished, cancellationToken),
                FeaturedProjects = await _unitOfWork.Repository<Project>().CountAsync(x => x.IsFeatured, cancellationToken),
                TotalExperiences = await _unitOfWork.Repository<Experience>().CountAsync(null, cancellationToken),
                TotalSkills = await _unitOfWork.Repository<Skill>().CountAsync(null, cancellationToken),
                TotalContactLinks = await _unitOfWork.Repository<ContactLink>().CountAsync(null, cancellationToken)
            };

            return View(viewModel);
        }
    }
}
