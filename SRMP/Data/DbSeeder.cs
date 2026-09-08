using Microsoft.EntityFrameworkCore;
using SRMP.Helpers;
using SRMP.Models;

namespace SRMP.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(
            AppDbContext context,
            IConfiguration configuration)
        {
            var adminEmail = configuration["SeedAdmin:Email"];

            if (string.IsNullOrWhiteSpace(adminEmail))
            {
                throw new InvalidOperationException(
                    "Admin email configuration is missing.");
            }

            adminEmail = adminEmail.Trim().ToLowerInvariant();

            var existingAdmin = await context.Users
                .FirstOrDefaultAsync(u => u.Email == adminEmail);

            if (existingAdmin != null)
            {
                return;
            }

            var adminName = configuration["SeedAdmin:Name"];
            var adminPassword = configuration["SeedAdmin:Password"];

            if (string.IsNullOrWhiteSpace(adminName) ||
                string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new InvalidOperationException(
                    "Admin seed configuration is missing.");
            }

            var admin = new User
            {
                FullName = adminName.Trim(),
                Email = adminEmail,
                Role = UserRole.Administrator,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            admin.PasswordHash =
                PasswordHelper.HashPassword(
                    admin,
                    adminPassword);

            context.Users.Add(admin);

            await context.SaveChangesAsync();
        }
    }
}