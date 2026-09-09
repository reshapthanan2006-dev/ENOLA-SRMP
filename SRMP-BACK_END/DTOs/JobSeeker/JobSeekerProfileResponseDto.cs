namespace SRMP.DTOs.JobSeeker
{
    public class JobSeekerProfileResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public List<string> Skills { get; set; } = new();

        public int ExperienceYears { get; set; }

        public string Education { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;
    }
}