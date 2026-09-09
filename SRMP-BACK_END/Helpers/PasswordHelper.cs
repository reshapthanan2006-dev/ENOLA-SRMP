using Microsoft.AspNetCore.Identity;
using SRMP.Models;

namespace SRMP.Helpers
{
    public static class PasswordHelper
    {
        private static readonly PasswordHasher<User> PasswordHasher =
            new PasswordHasher<User>();

        public static string HashPassword(
            User user,
            string password)
        {
            return PasswordHasher.HashPassword(user, password);
        }

        public static bool VerifyPassword(
            User user,
            string hashedPassword,
            string password)
        {
            var result =
                PasswordHasher.VerifyHashedPassword(
                    user,
                    hashedPassword,
                    password);

            return result == PasswordVerificationResult.Success ||
                   result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}