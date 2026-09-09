using SRMP.Models;

namespace SRMP.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetByJobSeekerAsync(int jobSeekerId);

        Task AddAsync(Notification notification);

        Task<Notification?> GetByIdAsync(int notificationId);

        Task UpdateAsync(Notification notification);
    }
}