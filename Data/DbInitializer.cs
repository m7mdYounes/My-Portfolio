using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyPortfolio.Models.Identity;
using MyPortfolio.Options;

namespace MyPortfolio.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var environment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            var adminOptions = scope.ServiceProvider.GetRequiredService<IOptions<SeedAdminOptions>>().Value;

            await dbContext.Database.MigrateAsync();

            await SeedRolesAsync(roleManager);

            if (!adminOptions.SeedEnabled)
                return;

            await SeedAdminUserAsync(
                userManager,
                adminOptions,
                environment);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            const string adminRole = "Admin";

            if (await roleManager.RoleExistsAsync(adminRole))
                return;

            var result = await roleManager.CreateAsync(new IdentityRole(adminRole));

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create Admin role. Errors: {errors}");
            }
        }

        private static async Task SeedAdminUserAsync(
            UserManager<ApplicationUser> userManager,
            SeedAdminOptions adminOptions,
            IWebHostEnvironment environment)
        {
            ValidateSeedOptions(adminOptions);

            var adminUsers = await userManager.GetUsersInRoleAsync("Admin");
            var existingAdmin = adminUsers.FirstOrDefault();

            if (existingAdmin is null)
            {
                await CreateInitialAdminAsync(userManager, adminOptions);
                return;
            }

            if (adminOptions.UpdateExistingAdmin)
            {
                await UpdateExistingAdminAsync(userManager, existingAdmin, adminOptions);
            }

            if (adminOptions.ResetPasswordOnStartup)
            {
                if (!environment.IsDevelopment())
                {
                    throw new InvalidOperationException(
                        "ResetPasswordOnStartup is not allowed outside Development.");
                }

                await ResetAdminPasswordAsync(userManager, existingAdmin, adminOptions.Password);
            }
        }

        private static void ValidateSeedOptions(SeedAdminOptions adminOptions)
        {
            if (string.IsNullOrWhiteSpace(adminOptions.Email))
                throw new InvalidOperationException("SeedAdmin:Email is missing.");

            if (string.IsNullOrWhiteSpace(adminOptions.Password))
                throw new InvalidOperationException("SeedAdmin:Password is missing.");

            if (string.IsNullOrWhiteSpace(adminOptions.FullName))
                throw new InvalidOperationException("SeedAdmin:FullName is missing.");
        }

        private static async Task CreateInitialAdminAsync(
            UserManager<ApplicationUser> userManager,
            SeedAdminOptions adminOptions)
        {
            var existingUserByEmail = await userManager.FindByEmailAsync(adminOptions.Email);

            if (existingUserByEmail is not null)
            {
                if (!await userManager.IsInRoleAsync(existingUserByEmail, "Admin"))
                {
                    var addRoleResult = await userManager.AddToRoleAsync(existingUserByEmail, "Admin");

                    if (!addRoleResult.Succeeded)
                    {
                        var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Failed to assign Admin role. Errors: {errors}");
                    }
                }

                return;
            }

            var adminUser = new ApplicationUser
            {
                UserName = adminOptions.Email,
                Email = adminOptions.Email,
                EmailConfirmed = true,
                FullName = adminOptions.FullName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createResult = await userManager.CreateAsync(adminUser, adminOptions.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create admin user. Errors: {errors}");
            }

            var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to assign Admin role. Errors: {errors}");
            }
        }

        private static async Task UpdateExistingAdminAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationUser existingAdmin,
            SeedAdminOptions adminOptions)
        {
            var changed = false;

            if (!string.Equals(existingAdmin.Email, adminOptions.Email, StringComparison.OrdinalIgnoreCase))
            {
                existingAdmin.Email = adminOptions.Email;
                existingAdmin.UserName = adminOptions.Email;
                existingAdmin.NormalizedEmail = userManager.NormalizeEmail(adminOptions.Email);
                existingAdmin.NormalizedUserName = userManager.NormalizeName(adminOptions.Email);
                existingAdmin.EmailConfirmed = true;
                changed = true;
            }

            if (!string.Equals(existingAdmin.FullName, adminOptions.FullName, StringComparison.Ordinal))
            {
                existingAdmin.FullName = adminOptions.FullName;
                changed = true;
            }

            if (!changed)
                return;

            var updateResult = await userManager.UpdateAsync(existingAdmin);

            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to update admin user. Errors: {errors}");
            }
        }

        private static async Task ResetAdminPasswordAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationUser adminUser,
            string newPassword)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);

            var passwordResult = await userManager.ResetPasswordAsync(
                adminUser,
                token,
                newPassword);

            if (!passwordResult.Succeeded)
            {
                var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to reset admin password. Errors: {errors}");
            }
        }
    }
}
