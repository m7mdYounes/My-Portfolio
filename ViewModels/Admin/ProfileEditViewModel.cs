using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.ViewModels.Admin
{
    public class ProfileEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(1000)]
        public string ShortSummary { get; set; } = null!;

        [MaxLength(4000)]
        public string? LongSummary { get; set; }

        public string? CurrentProfileImagePath { get; set; }

        public IFormFile? ProfileImage { get; set; }

        public string? CurrentCvFilePath { get; set; }

        public IFormFile? CvFile { get; set; }

        [MaxLength(150)]
        public string? Location { get; set; }

        [Range(0, 50)]
        public int YearsOfExperience { get; set; }

        public bool IsAvailableForWork { get; set; }
    }
}
