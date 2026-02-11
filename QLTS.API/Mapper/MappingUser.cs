using AutoMapper;
using Microsoft.AspNetCore.Identity;
using QLTS.Core.Entities;
using QLTS.Infrastructure.Data.Dtos;

namespace QLTS.API.Mapper
{
    public class MappingUser : Profile
    {
        public MappingUser()
        {
            CreateMap<AppUser, UserDto>()
                .ForMember(a => a.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(a => a.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(a => a.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(a => a.Code, opt => opt.MapFrom(src => src.Code));
            CreateMap<AppUser, RegisterDto>()
                .ForMember(a => a.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(a => a.Email, opt => opt.MapFrom(src => src.Email));
            CreateMap<AppUser, GetUserDto>()
                .ForMember(a => a.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(a => a.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(a => a.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(a => a.Code, opt => opt.MapFrom(src => src.Code));
        }
    }
}
