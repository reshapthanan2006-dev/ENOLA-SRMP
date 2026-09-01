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
            var totalUsers =
                await _adminRepository.GetTotalUsersAsync();

            var totalVacancies =
                await _adminRepository.GetTotalVacanciesAsync();

            var totalApplications =
                await _adminRepository.GetTotalApplicationsAsync();

            return new AdminDashboardDto
            {
                TotalUsers = totalUsers,
                TotalVacancies = totalVacancies,
                TotalApplications = totalApplications
            };
        }

        public async Task<List<AdminUserDto>> GetUsersAsync()
        {
            var users =
                await _adminRepository.GetAllUsersAsync();

            return users.Select(user => new AdminUserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            }).ToList();
        }

        public async Task<AdminUserDto> UpdateUserStatusAsync(
            int userId,
            UpdateUserStatusDto updateDto)
        {
            var user =
                await _adminRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException(
                    "User not found."
                );
            }

            user.IsActive = updateDto.IsActive;

            await _adminRepository.UpdateUserAsync(user);

            return new AdminUserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}