namespace MyPortfolio.Options
{
    public class SeedAdminOptions
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public bool SeedEnabled { get; set; } = true;

        public bool UpdateExistingAdmin { get; set; } = false;

        public bool ResetPasswordOnStartup { get; set; } = false;
    }
}
