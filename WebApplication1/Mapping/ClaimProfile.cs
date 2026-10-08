using AutoMapper;
using WebApplication1.Models.Identity;
using WebApplication1.Models.ViewModel.Identity.Claims;

namespace WebApplication1.Mapping
{
    public class ClaimProfile : Profile
    {
        public ClaimProfile()
        {
            CreateMap<Claim, GetClaimsListViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.localize(src.NameEn, src.NameAr)));

            CreateMap<AddClaimViewModel, Claim>();

            CreateMap<Claim, UpdateClaimViewModel>().ReverseMap();

            CreateMap<Claim,GetClaimByIdViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.localize(src.NameEn, src.NameAr)));

        }
    }
}
