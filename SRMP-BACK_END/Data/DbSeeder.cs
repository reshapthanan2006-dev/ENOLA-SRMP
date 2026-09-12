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
            var adminName = configuration["SeedAdmin:FullName"];
            var adminPassword = configuration["SeedAdmin:Password"];

     
            if (string.IsNullOrWhiteSpace(adminEmail) &&
                string.IsNullOrWhiteSpace(adminName) &&
                string.IsNullOrWhiteSpace(adminPassword))
            {
                return;
            }

             if (string.IsNullOrWhiteSpace(adminEmail) ||
                string.IsNullOrWhiteSpace(adminName) ||
                string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new InvalidOperationException(
                    "Admin seed configuration is incomplete.");
            }

            adminEmail =
                adminEmail.Trim().ToLowerInvariant();

            var existingUser = await context.Users
                .FirstOrDefaultAsync(
                    u => u.Email == adminEmail);

            if (existingUser != null)
            {
                if (existingUser.Role != UserRole.Administrator)
                {
                    throw new InvalidOperationException(
                        "The configured admin email already belongs to a non-admin user.");
                }

                return;
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