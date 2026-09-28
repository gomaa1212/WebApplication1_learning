namespace WebApplication1.Models.ViewModel.Students
{
    public class GetAllStudentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? FileUrl { get; set; }
        public string GenderName { get; set; }
        public List<StudentImages>? StudentImages { get; set; }

    }
}
