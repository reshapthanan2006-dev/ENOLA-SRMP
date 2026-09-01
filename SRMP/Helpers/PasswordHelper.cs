using Microsoft.AspNetCore.Identity;
using SRMP.Models;

namespace SRMP.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(User user, string password)
        {
            var passwordHasher = new PasswordHasher<User>();

            return passwordHasher.HashPassword(user, password);
        }

        public static bool VerifyPassword(
            User user,
            string hashedPassword,
            string password)
        {
            var passwordHasher = new PasswordHasher<User>();

            var result = passwordHasher.VerifyHashedPassword(
                user,
                hashedPassword,
                password
            );

            return result == PasswordVerificationResult.Success ||
                   result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}