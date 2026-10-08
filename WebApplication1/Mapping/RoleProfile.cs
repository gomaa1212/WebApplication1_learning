using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Helper;
using WebApplication1.Models.Identity;
using WebApplication1.Models.ViewModel.Identity.Role;

namespace WebApplication1.Mapping
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            
            CreateMap<User, GetUsersInRoleViewModel>()
                .ForMember(des => des.FullName, opt => opt.MapFrom(src => CultureHelper.IsEnglish() ? src.FullNameEn : src.FullNameAr))
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(des => des.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<IdentityRole, GetRolesViewModel>().ReverseMap();
            CreateMap<IdentityRole, AddRoleViewModel>().ReverseMap();
            CreateMap<IdentityRole, UpdateRoleViewModel>().ReverseMap();


        }
    }
}
