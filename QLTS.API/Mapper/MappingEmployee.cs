using AutoMapper;
using QLTS.Core.Entities;
using QLTS.Infrastructure.Data.Dtos;

namespace QLTS.API.Mapper
{
    public class MappingEmployee : Profile
    {
        public MappingEmployee()
        {
            CreateMap<EmployeeDto, Employees>().ReverseMap();
            CreateMap<Employees, EmployeeDto>()
                .ForMember(d => d.DeptId,
                           o => o.MapFrom(s => s.DeptId))
                .ForMember(d => d.DeptName,
                           o => o.MapFrom(s => s.Departments.Name))
                .ReverseMap()
                .ForMember(d => d.Departments, o => o.Ignore());
            CreateMap<Employees, AddEmployeeDto>()
                .ForMember(d => d.DeptId,
                           o => o.MapFrom(s => s.DeptId))
                
                .ReverseMap();
            
        }
    }
}
