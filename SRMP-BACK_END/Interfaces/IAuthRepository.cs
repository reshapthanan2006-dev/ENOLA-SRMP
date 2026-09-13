using SRMP.Models;

namespace SRMP.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByResetPasswordTokenHashAsync(
            string tokenHash);

        Task<bool> EmailExistsAsync(string email);

        Task<User> CreateUserAsync(User user);

        Task UpdateUserAsync(User user);
    }
}