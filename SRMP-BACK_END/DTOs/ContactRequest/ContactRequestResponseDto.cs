namespace SRMP.DTOs
{
    public class ContactRequestResponseDto
    {
        public int ContactRequestId { get; set; }

        public int EmployerId { get; set; }

        public int JobSeekerId { get; set; }

        public int ApplicationId { get; set; }

        public int JobVacancyId { get; set; }

        public string CompanyName { get; set; }
            = string.Empty;

        public string JobTitle { get; set; }
            = string.Empty;

        public string JobLocation { get; set; }
            = string.Empty;

        public string ApplicationStatus { get; set; }
            = string.Empty;

        public string Status { get; set; }
            = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}