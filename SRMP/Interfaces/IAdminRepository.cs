using SRMP.Models;

namespace SRMP.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<User>> GetAllUsersAsync();

        Task<User?> GetUserByIdAsync(int userId);

        Task<int> GetTotalUsersAsync();

        Task<int> GetTotalVacanciesAsync();

        Task<int> GetTotalApplicationsAsync();

        Task<User> UpdateUserAsync(User user);
    }
}