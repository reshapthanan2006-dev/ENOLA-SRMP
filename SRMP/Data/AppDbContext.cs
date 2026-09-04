using Microsoft.EntityFrameworkCore;
using SRMP.Models;

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
        }
    }
}