using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Models.Content;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.ViewModels.Admin;

namespace MyPortfolio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("admin/contact-links")]
    public class ContactLinksController : Controller
    {
        private readonly IContactLinkService _contactLinkService;

        public ContactLinksController(IContactLinkService contactLinkService)
        {
            _contactLinkService = contactLinkService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var contactLinks = await _contactLinkService.GetAllForAdminAsync(cancellationToken);

            return View(contactLinks);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new ContactLinkFormViewModel
            {
                IsPublished = true
            });
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ContactLinkFormViewModel model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            var contactLink = new ContactLink
            {
                Type = model.Type,
                Label = model.Label,
                Value = model.Value,
                Url = model.Url,
                Icon = model.Icon,
                DisplayOrder = model.DisplayOrder,
                IsPublished = model.IsPublished
            };

            var result = await _contactLinkService.CreateAsync(contactLink, cancellationToken);

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
            var contactLink = await _contactLinkService.GetByIdAsync(id, cancellationToken);

            if (contactLink is null)
                return NotFound();

            var model = new ContactLinkFormViewModel
            {
                Id = contactLink.Id,
                Type = contactLink.Type,
                Label = contactLink.Label,
                Value = contactLink.Value,
                Url = contactLink.Url,
                Icon = contactLink.Icon,
                DisplayOrder = contactLink.DisplayOrder,
                IsPublished = contactLink.IsPublished
            };

            return View(model);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            ContactLinkFormViewModel model,
            CancellationToken cancellationToken)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var contactLink = new ContactLink
            {
                Id = model.Id,
                Type = model.Type,
                Label = model.Label,
                Value = model.Value,
                Url = model.Url,
                Icon = model.Icon,
                DisplayOrder = model.DisplayOrder,
                IsPublished = model.IsPublished
            };

            var result = await _contactLinkService.UpdateAsync(contactLink, cancellationToken);

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
            var result = await _contactLinkService.DeleteAsync(id, cancellationToken);

            TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] =
                result.Succeeded ? result.Message : string.Join(", ", result.Errors);

            return RedirectToAction(nameof(Index));
        }
    }
}
