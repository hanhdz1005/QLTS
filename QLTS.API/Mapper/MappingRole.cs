using AutoMapper;
using Microsoft.AspNetCore.Identity;
using QLTS.Core.Entities;
using QLTS.Infrastructure.Data.Dtos;

namespace QLTS.API.Mapper
{
    public class MappingRole : Profile
    {
        public MappingRole()
        {
            CreateMap<IdentityRole, RolesDto>()
                .ForMember(r => r.Code, opt => opt.MapFrom(src => src.NormalizedName));
            CreateMap<IdentityRole, AddRolesDto>();
        }
    }
}
