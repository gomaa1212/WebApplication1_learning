using AutoMapper;
using WebApplication1.Models;
using WebApplication1.Models.ViewModel.Genderss;
using WebApplication1.Models.ViewModel.Students;

namespace WebApplication1.Mapping
{
    public class GenderProfile : Profile
    {
        public GenderProfile()
        {
            CreateMap<Gender, GetAllGenderViewModel>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.localize(src.NameEn, src.NameAr)))
                .ForMember(des => des.students, opt => opt.MapFrom(src => src.Students));

            CreateMap<Gender, UpdateGenderViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr));

            CreateMap<UpdateGenderViewModel, Gender>();

            CreateMap<AddGenderViewModel, Gender>();
        }
    }
}
