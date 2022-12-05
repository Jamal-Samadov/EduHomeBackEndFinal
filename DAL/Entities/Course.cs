namespace EduHome.DAL.Entities
{
    public class Course : Entity
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
        public string Apply { get; set; }
        public string Certification { get; set; }
        public string Reply { get; set; }
        public DateTime Starts { get; set; }
        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public int Student { get; set; }
        public string Assesment { get; set; }
        public int CourseFee { get; set; }
    }
}
