using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Models.Analytics;
using MyPortfolio.Models.Content;
using MyPortfolio.Models.Identity;

namespace MyPortfolio.Data
{
    public class ApplicationDbContext
     : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Profile> Profiles => Set<Profile>();

        public DbSet<Experience> Experiences => Set<Experience>();
        public DbSet<ExperienceResponsibility> ExperienceResponsibilities => Set<ExperienceResponsibility>();

        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectImage> ProjectImages => Set<ProjectImage>();

        public DbSet<SkillCategory> SkillCategories => Set<SkillCategory>();
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<ProjectSkill> ProjectSkills => Set<ProjectSkill>();

        public DbSet<ContactLink> ContactLinks => Set<ContactLink>();
        public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();

        public DbSet<Visitor> Visitors => Set<Visitor>();
        public DbSet<VisitorSession> VisitorSessions => Set<VisitorSession>();
        public DbSet<PageView> PageViews => Set<PageView>();
        public DbSet<ClickEvent> ClickEvents => Set<ClickEvent>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureIdentity(builder);
            ConfigureContent(builder);
            ConfigureAnalytics(builder);
            SeedStaticData(builder);
        }

        private static void ConfigureIdentity(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(x => x.FullName)
                    .HasMaxLength(150);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
            });
        }

        private static void ConfigureContent(ModelBuilder builder)
        {
            builder.Entity<Profile>(entity =>
            {
                entity.ToTable("Profiles");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.FullName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.ShortSummary)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(x => x.LongSummary)
                    .HasMaxLength(4000);

                entity.Property(x => x.ProfileImagePath)
                    .HasMaxLength(500);

                entity.Property(x => x.CvFilePath)
                    .HasMaxLength(500);

                entity.Property(x => x.Location)
                    .HasMaxLength(150);

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasIndex(x => x.FullName);
            });

            builder.Entity<Experience>(entity =>
            {
                entity.ToTable("Experiences");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.CompanyName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.JobTitle)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.EmploymentType)
                    .HasMaxLength(100);

                entity.Property(x => x.Location)
                    .HasMaxLength(150);

                entity.Property(x => x.CompanyLogoPath)
                    .HasMaxLength(500);

                entity.Property(x => x.IsPublished)
                    .HasDefaultValue(true);

                entity.HasMany(x => x.Responsibilities)
                    .WithOne(x => x.Experience)
                    .HasForeignKey(x => x.ExperienceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => x.DisplayOrder);
                entity.HasIndex(x => x.IsPublished);
                entity.HasIndex(x => new { x.IsPublished, x.DisplayOrder });
            });

            builder.Entity<ExperienceResponsibility>(entity =>
            {
                entity.ToTable("ExperienceResponsibilities");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Text)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.HasIndex(x => x.ExperienceId);
                entity.HasIndex(x => new { x.ExperienceId, x.DisplayOrder });
            });

            builder.Entity<Project>(entity =>
            {
                entity.ToTable("Projects");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.Slug)
                    .IsRequired()
                    .HasMaxLength(220);

                entity.Property(x => x.ShortDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(x => x.FullDescription)
                    .HasMaxLength(8000);

                entity.Property(x => x.ProblemStatement)
                    .HasMaxLength(4000);

                entity.Property(x => x.SolutionOverview)
                    .HasMaxLength(4000);

                entity.Property(x => x.TechnicalHighlights)
                    .HasMaxLength(4000);

                entity.Property(x => x.MainImagePath)
                    .HasMaxLength(500);

                entity.Property(x => x.DemoUrl)
                    .HasMaxLength(1000);

                entity.Property(x => x.GitHubUrl)
                    .HasMaxLength(1000);

                entity.Property(x => x.IsPublished)
                    .HasDefaultValue(true);

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasMany(x => x.Images)
                    .WithOne(x => x.Project)
                    .HasForeignKey(x => x.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(x => x.ProjectSkills)
                    .WithOne(x => x.Project)
                    .HasForeignKey(x => x.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => x.Slug)
                    .IsUnique();

                entity.HasIndex(x => x.IsPublished);
                entity.HasIndex(x => x.IsFeatured);
                entity.HasIndex(x => x.DisplayOrder);
                entity.HasIndex(x => new { x.IsPublished, x.DisplayOrder });
                entity.HasIndex(x => new { x.IsPublished, x.IsFeatured, x.DisplayOrder });
            });

            builder.Entity<ProjectImage>(entity =>
            {
                entity.ToTable("ProjectImages");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.ImagePath)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(x => x.Caption)
                    .HasMaxLength(300);

                entity.HasIndex(x => x.ProjectId);
                entity.HasIndex(x => new { x.ProjectId, x.DisplayOrder });
            });

            builder.Entity<SkillCategory>(entity =>
            {
                entity.ToTable("SkillCategories");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasMany(x => x.Skills)
                    .WithOne(x => x.SkillCategory)
                    .HasForeignKey(x => x.SkillCategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => x.Name)
                    .IsUnique();

                entity.HasIndex(x => x.DisplayOrder);
            });

            builder.Entity<Skill>(entity =>
            {
                entity.ToTable("Skills");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.IconPath)
                    .HasMaxLength(500);

                entity.Property(x => x.Level)
                    .HasMaxLength(50);

                entity.Property(x => x.IsPublished)
                    .HasDefaultValue(true);

                entity.HasMany(x => x.ProjectSkills)
                    .WithOne(x => x.Skill)
                    .HasForeignKey(x => x.SkillId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => x.SkillCategoryId);
                entity.HasIndex(x => x.IsPublished);
                entity.HasIndex(x => x.DisplayOrder);
                entity.HasIndex(x => new { x.SkillCategoryId, x.Name })
                    .IsUnique();

                entity.HasIndex(x => new { x.SkillCategoryId, x.IsPublished, x.DisplayOrder });
            });

            builder.Entity<ProjectSkill>(entity =>
            {
                entity.ToTable("ProjectSkills");

                entity.HasKey(x => new { x.ProjectId, x.SkillId });

                entity.HasIndex(x => x.ProjectId);
                entity.HasIndex(x => x.SkillId);
            });

            builder.Entity<ContactLink>(entity =>
            {
                entity.ToTable("ContactLinks");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Type)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Label)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(x => x.Value)
                    .HasMaxLength(300);

                entity.Property(x => x.Url)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(x => x.Icon)
                    .HasMaxLength(100);

                entity.Property(x => x.IsPublished)
                    .HasDefaultValue(true);

                entity.HasIndex(x => x.Type);
                entity.HasIndex(x => x.IsPublished);
                entity.HasIndex(x => x.DisplayOrder);
                entity.HasIndex(x => new { x.IsPublished, x.DisplayOrder });
            });

            builder.Entity<SiteSetting>(entity =>
            {
                entity.ToTable("SiteSettings");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.SiteTitle)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.MetaDescription)
                    .HasMaxLength(500);

                entity.Property(x => x.LogoPath)
                    .HasMaxLength(500);

                entity.Property(x => x.FaviconPath)
                    .HasMaxLength(500);

                entity.Property(x => x.PrimaryColor)
                    .HasMaxLength(20);

                entity.Property(x => x.SecondaryColor)
                    .HasMaxLength(20);

                entity.Property(x => x.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
        }

        private static void ConfigureAnalytics(ModelBuilder builder)
        {
            builder.Entity<Visitor>(entity =>
            {
                entity.ToTable("Visitors");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.VisitorKey)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.FirstIpAddressHash)
                    .HasMaxLength(256);

                entity.Property(x => x.LastIpAddressHash)
                    .HasMaxLength(256);

                entity.Property(x => x.UserAgent)
                    .HasMaxLength(1000);

                entity.Property(x => x.Browser)
                    .HasMaxLength(100);

                entity.Property(x => x.OperatingSystem)
                    .HasMaxLength(100);

                entity.Property(x => x.DeviceType)
                    .HasMaxLength(100);

                entity.Property(x => x.FirstVisitAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(x => x.LastVisitAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasMany(x => x.Sessions)
                    .WithOne(x => x.Visitor)
                    .HasForeignKey(x => x.VisitorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(x => x.PageViews)
                    .WithOne(x => x.Visitor)
                    .HasForeignKey(x => x.VisitorId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(x => x.ClickEvents)
                    .WithOne(x => x.Visitor)
                    .HasForeignKey(x => x.VisitorId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasIndex(x => x.VisitorKey)
                    .IsUnique();

                entity.HasIndex(x => x.FirstVisitAt);
                entity.HasIndex(x => x.LastVisitAt);
            });

            builder.Entity<VisitorSession>(entity =>
            {
                entity.ToTable("VisitorSessions");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.SessionKey)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.IpAddressHash)
                    .HasMaxLength(256);

                entity.Property(x => x.UserAgent)
                    .HasMaxLength(1000);

                entity.Property(x => x.Referrer)
                    .HasMaxLength(1000);

                entity.Property(x => x.StartedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(x => x.LastActivityAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasMany(x => x.PageViews)
                    .WithOne(x => x.VisitorSession)
                    .HasForeignKey(x => x.VisitorSessionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(x => x.ClickEvents)
                    .WithOne(x => x.VisitorSession)
                    .HasForeignKey(x => x.VisitorSessionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => x.SessionKey)
                    .IsUnique();

                entity.HasIndex(x => x.VisitorId);
                entity.HasIndex(x => x.StartedAt);
                entity.HasIndex(x => x.LastActivityAt);
                entity.HasIndex(x => new { x.VisitorId, x.LastActivityAt });
            });

            builder.Entity<PageView>(entity =>
            {
                entity.ToTable("PageViews");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Path)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(x => x.FullUrl)
                    .HasMaxLength(1000);

                entity.Property(x => x.PageTitle)
                    .HasMaxLength(300);

                entity.Property(x => x.Referrer)
                    .HasMaxLength(1000);

                entity.Property(x => x.IpAddressHash)
                    .HasMaxLength(256);

                entity.Property(x => x.UserAgent)
                    .HasMaxLength(1000);

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasIndex(x => x.VisitorId);
                entity.HasIndex(x => x.VisitorSessionId);
                entity.HasIndex(x => x.Path);
                entity.HasIndex(x => x.CreatedAt);
                entity.HasIndex(x => new { x.Path, x.CreatedAt });
                entity.HasIndex(x => new { x.VisitorId, x.CreatedAt });
            });

            builder.Entity<ClickEvent>(entity =>
            {
                entity.ToTable("ClickEvents");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.EventType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.ComponentName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.TargetType)
                    .HasMaxLength(100);

                entity.Property(x => x.TargetId)
                    .HasMaxLength(100);

                entity.Property(x => x.TargetText)
                    .HasMaxLength(300);

                entity.Property(x => x.PagePath)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(x => x.MetadataJson)
                    .HasMaxLength(4000);

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasIndex(x => x.VisitorId);
                entity.HasIndex(x => x.VisitorSessionId);
                entity.HasIndex(x => x.EventType);
                entity.HasIndex(x => x.ComponentName);
                entity.HasIndex(x => x.PagePath);
                entity.HasIndex(x => x.CreatedAt);
                entity.HasIndex(x => new { x.EventType, x.CreatedAt });
                entity.HasIndex(x => new { x.ComponentName, x.CreatedAt });
                entity.HasIndex(x => new { x.PagePath, x.CreatedAt });
            });
        }

        private static void SeedStaticData(ModelBuilder builder)
        {
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.Entity<Profile>().HasData(new Profile
            {
                Id = 1,
                FullName = "Mohamed Younes Krema",
                Title = ".NET Backend Engineer",
                ShortSummary = ".NET Backend Engineer building secure, scalable APIs and backend systems using ASP.NET Core, SQL Server, and clean architecture.",
                LongSummary = "Software engineer focused on backend systems, APIs, SQL Server performance, authentication, authorization, and production-grade web applications.",
                Location = "Egypt",
                YearsOfExperience = 3,
                IsAvailableForWork = true,
                CreatedAt = seedDate
            });

            builder.Entity<SiteSetting>().HasData(new SiteSetting
            {
                Id = 1,
                SiteTitle = "Mohamed Younes | .NET Backend Engineer",
                MetaDescription = "Portfolio of Mohamed Younes Krema, .NET Backend Engineer specializing in ASP.NET Core, SQL Server, APIs, and clean architecture.",
                PrimaryColor = "#0ea5e9",
                SecondaryColor = "#111827",
                EnableDarkMode = true,
                UpdatedAt = seedDate
            });

            builder.Entity<SkillCategory>().HasData(
                new SkillCategory { Id = 1, Name = "Backend", DisplayOrder = 1 },
                new SkillCategory { Id = 2, Name = "Database", DisplayOrder = 2 },
                new SkillCategory { Id = 3, Name = "Architecture", DisplayOrder = 3 },
                new SkillCategory { Id = 4, Name = "Security", DisplayOrder = 4 },
                new SkillCategory { Id = 5, Name = "Tools", DisplayOrder = 5 },
                new SkillCategory { Id = 6, Name = "Frontend", DisplayOrder = 6 }
            );

            builder.Entity<Skill>().HasData(
                new Skill { Id = 1, SkillCategoryId = 1, Name = "C#", Level = "Advanced", DisplayOrder = 1, IsPublished = true },
                new Skill { Id = 2, SkillCategoryId = 1, Name = "ASP.NET Core", Level = "Advanced", DisplayOrder = 2, IsPublished = true },
                new Skill { Id = 3, SkillCategoryId = 1, Name = "ASP.NET Core MVC", Level = "Advanced", DisplayOrder = 3, IsPublished = true },
                new Skill { Id = 4, SkillCategoryId = 1, Name = "Web API", Level = "Advanced", DisplayOrder = 4, IsPublished = true },
                new Skill { Id = 5, SkillCategoryId = 1, Name = "Entity Framework Core", Level = "Advanced", DisplayOrder = 5, IsPublished = true },

                new Skill { Id = 6, SkillCategoryId = 2, Name = "SQL Server", Level = "Advanced", DisplayOrder = 1, IsPublished = true },
                new Skill { Id = 7, SkillCategoryId = 2, Name = "Query Optimization", Level = "Advanced", DisplayOrder = 2, IsPublished = true },
                new Skill { Id = 8, SkillCategoryId = 2, Name = "Database Design", Level = "Advanced", DisplayOrder = 3, IsPublished = true },

                new Skill { Id = 9, SkillCategoryId = 3, Name = "Clean Architecture", Level = "Advanced", DisplayOrder = 1, IsPublished = true },
                new Skill { Id = 10, SkillCategoryId = 3, Name = "SOLID Principles", Level = "Advanced", DisplayOrder = 2, IsPublished = true },
                new Skill { Id = 11, SkillCategoryId = 3, Name = "Repository Pattern", Level = "Advanced", DisplayOrder = 3, IsPublished = true },

                new Skill { Id = 12, SkillCategoryId = 4, Name = "JWT", Level = "Advanced", DisplayOrder = 1, IsPublished = true },
                new Skill { Id = 13, SkillCategoryId = 4, Name = "RBAC", Level = "Advanced", DisplayOrder = 2, IsPublished = true },

                new Skill { Id = 14, SkillCategoryId = 5, Name = "Git", Level = "Advanced", DisplayOrder = 1, IsPublished = true },
                new Skill { Id = 15, SkillCategoryId = 5, Name = "Swagger", Level = "Advanced", DisplayOrder = 2, IsPublished = true },
                new Skill { Id = 16, SkillCategoryId = 5, Name = "Postman", Level = "Advanced", DisplayOrder = 3, IsPublished = true },

                new Skill { Id = 17, SkillCategoryId = 6, Name = "HTML", Level = "Intermediate", DisplayOrder = 1, IsPublished = true },
                new Skill { Id = 18, SkillCategoryId = 6, Name = "CSS", Level = "Intermediate", DisplayOrder = 2, IsPublished = true },
                new Skill { Id = 19, SkillCategoryId = 6, Name = "Bootstrap", Level = "Intermediate", DisplayOrder = 3, IsPublished = true },
                new Skill { Id = 20, SkillCategoryId = 6, Name = "JavaScript", Level = "Intermediate", DisplayOrder = 4, IsPublished = true }
            );

            builder.Entity<ContactLink>().HasData(
                new ContactLink
                {
                    Id = 1,
                    Type = "Email",
                    Label = "Email",
                    Value = "mohammedyonis1997@gmail.com",
                    Url = "mailto:mohammedyonis1997@gmail.com",
                    Icon = "bi bi-envelope",
                    DisplayOrder = 1,
                    IsPublished = true
                },
                new ContactLink
                {
                    Id = 2,
                    Type = "WhatsApp",
                    Label = "WhatsApp",
                    Value = "+201141704109",
                    Url = "https://wa.me/201141704109",
                    Icon = "bi bi-whatsapp",
                    DisplayOrder = 2,
                    IsPublished = true
                },
                new ContactLink
                {
                    Id = 3,
                    Type = "LinkedIn",
                    Label = "LinkedIn",
                    Value = "LinkedIn",
                    Url = "https://www.linkedin.com/in/mohamed-younes-85474013b/",
                    Icon = "bi bi-linkedin",
                    DisplayOrder = 3,
                    IsPublished = true
                },
                new ContactLink
                {
                    Id = 4,
                    Type = "GitHub",
                    Label = "GitHub",
                    Value = "GitHub",
                    Url = "https://github.com/m7mdYounes",
                    Icon = "bi bi-github",
                    DisplayOrder = 4,
                    IsPublished = true
                }
            );
        }
    }
}
