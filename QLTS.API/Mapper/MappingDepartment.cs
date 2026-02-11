using AutoMapper;
using QLTS.Core.Entities;
using QLTS.Infrastructure.Data.Dtos;

namespace QLTS.API.Mapper
{
    public class MappingDepartment : Profile
    {
        public MappingDepartment()
        {
            CreateMap<DepartmentDto, Departments>().ReverseMap();
            CreateMap<Departments, AddDepartmentDto>().ReverseMap();
        }
    }
}
