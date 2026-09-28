using WebApplication1.Helper;

namespace WebApplication1.Models
{
    public class Gender : LocalizableEntity
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; } 
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
