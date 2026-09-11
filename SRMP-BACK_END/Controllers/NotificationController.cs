using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRMP.Interfaces;
using System.Security.Claims;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "JobSeeker")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(
            INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // Job Seeker views own notifications
        [HttpGet("my")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var jobSeekerId = GetUserId();

            if (jobSeekerId == null)
                return Unauthorized();

            var result = await _notificationService
                .GetMyNotificationsAsync(jobSeekerId.Value);

            return Ok(result);
        }

        // Job Seeker marks own notification as read
        [HttpPut("{notificationId}/read")]
        public async Task<IActionResult> MarkAsRead(
            int notificationId)
        {
            var jobSeekerId = GetUserId();

            if (jobSeekerId == null)
                return Unauthorized();

            var result = await _notificationService
                .MarkAsReadAsync(
                    jobSeekerId.Value,
                    notificationId);

            if (!result)
            {
                return NotFound(new
                {
                    message =
                        "Notification not found or access denied."
                });
            }

            return Ok(new
            {
                message = "Notification marked as read."
            });
        }

        // Get logged-in User ID from JWT
        private int? GetUserId()
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return null;

            if (!int.TryParse(
                userIdClaim.Value,
                out var userId))
            {
                return null;
            }

            return userId;
        }
    }
}