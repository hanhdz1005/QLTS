using AutoMapper;
using QLTS.Core.Entities;
using QLTS.Infrastructure.Data.Dtos;

namespace QLTS.API.Mapper
{
    public class MappingCategory : Profile
    {
        public MappingCategory() 
        {
            CreateMap<CategoryDto, Categories>().ReverseMap();
            CreateMap<AddCategoryDto, Categories>().ReverseMap();
        }
    }
}
