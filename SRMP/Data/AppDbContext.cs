using Microsoft.EntityFrameworkCore;
using SRMP.Models;
using System.Text.Json;

namespace SRMP.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<JobVacancy> JobVacancies { get; set; }

        public DbSet<Application> Applications { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<ContactRequest> ContactRequests { get; set; }

        public DbSet<EmployerCompany> EmployerCompanies { get; set; }

        public DbSet<JobSeekerProfile> JobSeekerProfiles { get; set; }

        public DbSet<JobSeekerCv> JobSeekerCvs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Application>()
                .HasIndex(a => new { a.JobSeekerId, a.JobVacancyId })
                .IsUnique();

            // Application → Job Seeker
            modelBuilder.Entity<Application>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.JobSeekerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Application → Job Vacancy
            modelBuilder.Entity<Application>()
                .HasOne<JobVacancy>()
                .WithMany()
                .HasForeignKey(a => a.JobVacancyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification → Job Seeker
            modelBuilder.Entity<Notification>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(n => n.JobSeekerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification → Application
            modelBuilder.Entity<Notification>()
                .HasOne<Application>()
                .WithMany()
                .HasForeignKey(n => n.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Contact Request → Employer
            modelBuilder.Entity<ContactRequest>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.EmployerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Contact Request → Job Seeker
            modelBuilder.Entity<ContactRequest>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.JobSeekerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Contact Request → Application
            modelBuilder.Entity<ContactRequest>()
                .HasOne<Application>()
                .WithMany()
                .HasForeignKey(c => c.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            // EmployerCompany relationship
            modelBuilder.Entity<EmployerCompany>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.EmployerId)
                .OnDelete(DeleteBehavior.Restrict);

            // One company profile per employer
            modelBuilder.Entity<EmployerCompany>()
                .HasIndex(c => c.EmployerId)
                .IsUnique();

            // Job Seeker Profile relationship
            modelBuilder.Entity<JobSeekerProfile>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<JobSeekerProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One profile per Job Seeker
            modelBuilder.Entity<JobSeekerProfile>()
                .HasIndex(p => p.UserId)
                .IsUnique();

            // Job Seeker Profile → User
            modelBuilder.Entity<JobSeekerProfile>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<JobSeekerProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One profile per user
            modelBuilder.Entity<JobSeekerProfile>()
                .HasIndex(p => p.UserId)
                .IsUnique();

            modelBuilder.Entity<JobSeekerCv>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(cv => cv.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobSeekerCv>()
                .HasIndex(cv => cv.UserId)
                .IsUnique();

            // Store Skills as JSON
            modelBuilder.Entity<JobSeekerProfile>()
                .Property(p => p.Skills)
                .HasConversion(
                    skills => JsonSerializer.Serialize(
                        skills,
                        (JsonSerializerOptions?)null),
                    skills => JsonSerializer.Deserialize<List<string>>(
                        skills,
                        (JsonSerializerOptions?)null) ?? new List<string>()
                );
        }
    }
}