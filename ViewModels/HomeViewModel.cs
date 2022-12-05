using EduHome.DAL.Entities;

namespace EduHome.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider>? Sliders { get; set; } 
        public List<Choose>? Chooses { get; set; } 
        public List<Event>? Events { get; set; } 
        public List<Blog> Blogs { get; set; } 
        public List<Category> Categories { get; set; } 
        public List<Course> Courses { get; set; } 

    }
}
