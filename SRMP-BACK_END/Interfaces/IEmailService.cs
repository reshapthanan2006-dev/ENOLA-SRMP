namespace SRMP.Interfaces
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(
            string recipientEmail,
            string recipientName,
            string resetToken);
    }
}
