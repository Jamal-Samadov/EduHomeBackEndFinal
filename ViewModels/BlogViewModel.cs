using EduHome.DAL.Entities;

namespace EduHome.ViewModels
{
    public class BlogViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Tags> Tags { get; set; }
        public Blog Blog { get; set; }
    }
}
