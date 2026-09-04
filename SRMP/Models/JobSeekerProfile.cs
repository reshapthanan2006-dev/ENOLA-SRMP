namespace SRMP.Models
{
    public class JobSeekerProfile
    {
        public int Id { get; set; }
        public List<string> Skills { get; set; } = new();
        public int ExperienceYears { get; set; } 
        public string Education { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
