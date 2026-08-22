using AutoMapper;
using WebApplication1.Models.ViewModel;

namespace WebApplication1.Models
{
    public class StudentProfile :Profile
    {
        public StudentProfile()
        {
            CreateMap<AddStudentViewModel, Student>();

        }
    }
}
