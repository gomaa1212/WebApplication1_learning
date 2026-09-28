using AutoMapper;
using WebApplication1.Models;
using WebApplication1.Models.ViewModel.Students;

namespace WebApplication1.Mapping
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<AddStudentViewModel, Student>();

            CreateMap<Student, GetAllStudentViewModel>()
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.localize(src.NameEn, src.NameAr)))
                 .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.localize(src.Gender.NameEn, src.Gender.NameAr)));

            CreateMap<Student, UpdateStudentViewModel>()
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.FileUrl))
                .ForMember(dest => dest.GenderId, opt => opt.MapFrom(src => src.GenderId))
                .ForMember(dest => dest.currentImages, opt => opt.MapFrom(src => src.StudentImages.Select(img => img.FileUrl).ToList()));

            CreateMap<UpdateStudentViewModel, Student>();
        }
    }
}
