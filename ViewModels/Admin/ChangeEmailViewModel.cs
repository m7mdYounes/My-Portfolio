using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.ViewModels.Admin
{
    public class ChangeEmailViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Current Email")]
        public string CurrentEmail { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "New Email")]
        public string NewEmail { get; set; } = null!;
    }
}
