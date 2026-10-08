using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Helper;
using WebApplication1.Models.Identity;
using WebApplication1.Models.ViewModel.Identity;
using WebApplication1.Models.ViewModel.Identity.Role;
using WebApplication1.Models.ViewModel.Identity.User;

namespace WebApplication1.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterViewModel, User>();

            CreateMap<User, GetUsersViewModel>()
                .ForMember(des => des.FullName, opt => opt.MapFrom(src => CultureHelper.IsEnglish() ? src.FullNameEn : src.FullNameAr))
                .ForMember(des => des.Address, opt => opt.MapFrom(src => CultureHelper.IsEnglish() ? src.AddressEn : src.AddressAr));

            CreateMap<UpdateUserViewModel, User>().ReverseMap();

            CreateMap<User,GetUserByIdViewModel>()
                .ForMember(des => des.FullName, opt => opt.MapFrom(src => CultureHelper.IsEnglish() ? src.FullNameEn : src.FullNameAr))
                .ForMember(des => des.Address, opt => opt.MapFrom(src => CultureHelper.IsEnglish() ? src.AddressEn : src.AddressAr));

            CreateMap<User, ManageUsersInRoleViewModel>()
                .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(des => des.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<IdentityRole,ManageRolesInUserViewModel>()
                .ForMember(des => des.RoleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(des => des.RoleName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
