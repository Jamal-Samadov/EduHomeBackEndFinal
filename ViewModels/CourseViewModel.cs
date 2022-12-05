using EduHome.DAL.Entities;

namespace EduHome.ViewModels
{
    public class CourseViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Tags> Tags { get; set; }
        public Course Course { get; set; }
    }
}
