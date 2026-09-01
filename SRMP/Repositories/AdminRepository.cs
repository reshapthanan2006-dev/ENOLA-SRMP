using Microsoft.EntityFrameworkCore;
using SRMP.Data;
using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetTotalVacanciesAsync()
        {
            return await _context.JobVacancies.CountAsync();
        }

        public async Task<int> GetTotalApplicationsAsync()
        {
            return await _context.JobApplications.CountAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }
    }
}