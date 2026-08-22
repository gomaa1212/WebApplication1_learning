namespace WebApplication1.Models
{
    public class StudentImages
    {
        public int Id { get; set; }
        public string FileUrl { get; set; }= string.Empty;
        public int StudentId { get; set; }
        public Student? Student { get; set; }
    }
}
