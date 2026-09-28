using WebApplication1.Models.ViewModel.Students;

namespace WebApplication1.Models.ViewModel.Genderss
{
    public class GetAllGenderViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public IEnumerable<GetAllStudentViewModel>? students { get; set; }
    }
}
