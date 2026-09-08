namespace SRMP.Models
{
    public class Vacancy
    {
        public int Id { get; set; }

        public List<string> RequiredSkills { get; set; } = new();

        public int RequiredExperienceYears { get; set; }

        public string RequiredEducation { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;
    }
}