using SRMP.DTOs;

namespace SRMP.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationResponseDto>> GetMyNotificationsAsync(
            int jobSeekerId);

        Task CreateNotificationAsync(
            int jobSeekerId,
            int applicationId,
            string message);

        Task<bool> MarkAsReadAsync(
            int jobSeekerId,
            int notificationId);
    }
}