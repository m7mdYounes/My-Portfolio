using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Helpers.Interfaces;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.ViewModels.Public;

namespace MyPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly IExperienceService _experienceService;
        private readonly IProjectService _projectService;
        private readonly ISkillService _skillService;
        private readonly IContactLinkService _contactLinkService;
        private readonly IDateTimeHelper _dateTimeHelper;

        public HomeController(
            IProfileService profileService,
            IExperienceService experienceService,
            IProjectService projectService,
            ISkillService skillService,
            IContactLinkService contactLinkService,
            IDateTimeHelper dateTimeHelper)
        {
            _profileService = profileService;
            _experienceService = experienceService;
            _projectService = projectService;
            _skillService = skillService;
            _contactLinkService = contactLinkService;
            _dateTimeHelper = dateTimeHelper;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var profile = await _profileService.GetProfileAsync(cancellationToken);
            var experiences = await _experienceService.GetPublishedExperiencesAsync(cancellationToken);
            var featuredProjects = await _projectService.GetFeaturedProjectsAsync(cancellationToken);
            var skillCategories = await _skillService.GetPublishedGroupedSkillsAsync(cancellationToken);
            var contactLinks = await _contactLinkService.GetPublishedAsync(cancellationToken);

            var viewModel = new HomeViewModel
            {
                Profile = profile is null
                    ? null
                    : new ProfileSummaryViewModel
                    {
                        FullName = profile.FullName,
                        Title = profile.Title,
                        ShortSummary = profile.ShortSummary,
                        LongSummary = profile.LongSummary,
                        ProfileImagePath = profile.ProfileImagePath,
                        CvFilePath = profile.CvFilePath,
                        Location = profile.Location,
                        YearsOfExperience = profile.YearsOfExperience,
                        IsAvailableForWork = profile.IsAvailableForWork
                    },

                Experiences = experiences.Select(x => new ExperienceViewModel
                {
                    Id = x.Id,
                    CompanyName = x.CompanyName,
                    JobTitle = x.JobTitle,
                    EmploymentType = x.EmploymentType,
                    Location = x.Location,
                    CompanyLogoPath = x.CompanyLogoPath,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsCurrent = x.IsCurrent,
                    Period = _dateTimeHelper.FormatExperiencePeriod(x.StartDate, x.EndDate, x.IsCurrent),
                    Responsibilities = x.Responsibilities
                        .OrderBy(r => r.DisplayOrder)
                        .Select(r => r.Text)
                        .ToList()
                }).ToList(),

                FeaturedProjects = featuredProjects.Select(x => new ProjectCardViewModel
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
                }).ToList(),

                SkillCategories = skillCategories.Select(x => new SkillCategoryViewModel
                {
                    Name = x.Name,
                    Skills = x.Skills.Select(s => new SkillViewModel
                    {
                        Name = s.Name,
                        IconPath = s.IconPath,
                        Level = s.Level
                    }).ToList()
                }).ToList(),

                ContactLinks = contactLinks.Select(x => new ContactLinkViewModel
                {
                    Id = x.Id,
                    Type = x.Type,
                    Label = x.Label,
                    Value = x.Value,
                    Url = x.Url,
                    Icon = x.Icon
                }).ToList()
            };

            return View(viewModel);
        }
    }
}
