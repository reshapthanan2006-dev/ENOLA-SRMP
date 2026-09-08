using SRMP.DTOs;
using SRMP.Interfaces;

namespace SRMP.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<AdminDashboardDto> GetDashboardAsync()
        {
            return new AdminDashboardDto
            {
                TotalUsers = await _adminRepository.GetTotalUsersAsync(),
                TotalVacancies = await _adminRepository.GetTotalVacanciesAsync(),
                TotalApplications = await _adminRepository.GetTotalApplicationsAsync()
            };
        }

        public async Task<List<AdminUserDto>> GetAllUsersAsync()
        {
            var users = await _adminRepository.GetAllUsersAsync();

            return users.Select(user => new AdminUserDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            }).ToList();
        }

        public async Task<AdminUserDto> UpdateUserStatusAsync(
            int userId,
            UpdateUserStatusDto dto)
        {
            var user = await _adminRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException(
                    "User not found.");
            }

            user.IsActive = dto.IsActive;

            await _adminRepository.UpdateUserAsync(user);

            return new AdminUserDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}