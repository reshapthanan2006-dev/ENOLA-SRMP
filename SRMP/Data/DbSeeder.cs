using Microsoft.EntityFrameworkCore;
using SRMP.Helpers;
using SRMP.Models;

namespace SRMP.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            AppDbContext context,
            IConfiguration configuration)
        {
            // Check whether an Administrator already exists
            var adminExists = await context.Users
                .AnyAsync(u => u.Role == UserRole.Administrator);

            if (adminExists)
            {
                return;
            }

            var adminName =
                configuration["SeedAdmin:Name"];

            var adminEmail =
                configuration["SeedAdmin:Email"];

            var adminPassword =
                configuration["SeedAdmin:Password"];

            if (string.IsNullOrWhiteSpace(adminName) ||
                string.IsNullOrWhiteSpace(adminEmail) ||
                string.IsNullOrWhiteSpace(adminPassword))
            {
                return;
            }

            var admin = new User
            {
                Name = adminName.Trim(),
                Email = adminEmail.Trim().ToLowerInvariant(),
                Role = UserRole.Administrator,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            admin.PasswordHash =
                PasswordHelper.HashPassword(
                    admin,
                    adminPassword
                );

            await context.Users.AddAsync(admin);

            await context.SaveChangesAsync();
        }
    }
}