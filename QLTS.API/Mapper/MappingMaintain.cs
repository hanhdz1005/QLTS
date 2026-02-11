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
                .ForMember(dest => dest.EndDate, opt => opt.Ignore())
                .ForMember(dest => dest.TotalCost, opt => opt.Ignore());

            CreateMap<UpdateMaintainDto, Maintain>();
        }
    }
}
