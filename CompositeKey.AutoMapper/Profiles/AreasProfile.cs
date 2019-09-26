using AutoMapper;
using CompositeKey.Domain.ViewModels;
using CompositeKey.Model;

namespace CompositeKey.AutoMapper.Profiles
{
    public class AreasProfile: Profile
    {
        public AreasProfile() {
            CreateMap<Area, AreaViewModel>()
                .ForMember(dest => dest.CreatedBy, src => src.MapFrom(s => s.CreatedBy.UserName))
                .ForMember(dest => dest.UpdatedBy, src => src.MapFrom(s => s.UpdatedBy.UserName));

            CreateMap<AreaEditViewModel, Area>()
                .ForMember(dest => dest.Id, src => src.Ignore())
                .ForMember(dest => dest.CreatedOn, src => src.Ignore())
                .ForMember(dest => dest.CreatedById, src => src.Ignore());

            CreateMap<AreaAddViewModel, Area>();
        }
    }
}
