using AutoMapper;
using QLTS.Core.Entities;
using QLTS.Infrastructure.Data.Dtos;

namespace QLTS.API.Mapper
{
    public class MappingMaintain : Profile
    {
        public MappingMaintain()
        {
            CreateMap<Maintain, MaintainDto>();
            CreateMap<Maintain, GetMaintainDto>();

            CreateMap<CreateMaintainDto, Maintain>()
                .ForMember(m => m.EndDate, opt => opt.Ignore())
                .ForMember(m => m.TotalCost, opt => opt.Ignore());

            CreateMap<UpdateMaintainDto, Maintain>();

            //CreateMap<Maintain, UpdateMaintainDto>()
            //    .ForMember(u => u.AssetId, m => m.MapFrom(s => s.Asset.Id))
            //    .ForMember(u => u.CategoryId, );
        }
    }
}
