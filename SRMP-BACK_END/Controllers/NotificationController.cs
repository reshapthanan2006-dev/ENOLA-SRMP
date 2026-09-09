using Microsoft.AspNetCore.Mvc;
using SRMP.Interfaces;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // Job Seeker views notifications
        [HttpGet("my")]
        public async Task<IActionResult> GetMyNotifications(
            [FromQuery] int jobSeekerId)
        {
            var result = await _notificationService
                .GetMyNotificationsAsync(jobSeekerId);

            return Ok(result);
        }

        // Mark notification as read
        [HttpPut("{notificationId}/read")]
        public async Task<IActionResult> MarkAsRead(
            int notificationId,
            [FromQuery] int jobSeekerId)
        {
            var result = await _notificationService
                .MarkAsReadAsync(
                    jobSeekerId,
                    notificationId);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Notification not found or access denied."
                });
            }

            return Ok(new
            {
                message = "Notification marked as read."
            });
        }
    }
}