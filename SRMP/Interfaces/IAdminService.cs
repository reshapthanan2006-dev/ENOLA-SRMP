using SRMP.DTOs;

namespace SRMP.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardDto> GetDashboardAsync();

        Task<List<AdminUserDto>> GetAllUsersAsync();

        Task<AdminUserDto> UpdateUserStatusAsync(
            int userId,
            UpdateUserStatusDto dto);
    }
}