using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduHome.DAL.Entities
{
    public class Setting : Entity
    {
        [Required]
        public string Key { get; set; }
        public string? Value { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }
}
