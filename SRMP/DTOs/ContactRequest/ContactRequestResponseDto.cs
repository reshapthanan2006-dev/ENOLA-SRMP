namespace SRMP.DTOs
{
    public class ContactRequestResponseDto
    {
        public int ContactRequestId { get; set; }

        public int EmployerId { get; set; }

        public int JobSeekerId { get; set; }

        public int ApplicationId { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}