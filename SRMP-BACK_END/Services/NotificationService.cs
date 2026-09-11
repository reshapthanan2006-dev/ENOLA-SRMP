using SRMP.DTOs;
using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(
            INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<List<NotificationResponseDto>> GetMyNotificationsAsync(
            int jobSeekerId)
        {
            var notifications =
                await _notificationRepository.GetByJobSeekerAsync(jobSeekerId);

            return notifications.Select(n => new NotificationResponseDto
            {
                NotificationId = n.NotificationId,
                JobSeekerId = n.JobSeekerId,
                ApplicationId = n.ApplicationId,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            }).ToList();
        }

        public async Task CreateNotificationAsync(
            int jobSeekerId,
            int applicationId,
            string message)
        {
            var notification = new Notification
            {
                JobSeekerId = jobSeekerId,
                ApplicationId = applicationId,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);
        }

        public async Task<bool> MarkAsReadAsync(
            int jobSeekerId,
            int notificationId)
        {
            var notification =
                await _notificationRepository.GetByIdAsync(notificationId);

            if (notification == null)
            {
                return false;
            }

            // Only the owner of the notification can mark it as read
            if (notification.JobSeekerId != jobSeekerId)
            {
                return false;
            }

            notification.IsRead = true;

            await _notificationRepository.UpdateAsync(notification);

            return true;
        }
    }
}