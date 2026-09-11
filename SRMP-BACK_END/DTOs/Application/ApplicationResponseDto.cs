namespace SRMP.DTOs
{
    public class ApplicationResponseDto
    {
        public int ApplicationId { get; set; }

        public int JobSeekerId { get; set; }

        public int JobVacancyId { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime AppliedAt { get; set; }
    }
}