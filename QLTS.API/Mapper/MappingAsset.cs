using AutoMapper;
using QLTS.Core.Entities;
using QLTS.Infrastructure.Data.Dtos;

namespace QLTS.API.Mapper
{
    public class MappingAsset : Profile
    {
        public MappingAsset() 
        {
            CreateMap<AssetDto, Assets>().ReverseMap();
            CreateMap<Assets, AssetDto>()
                .ForMember(d => d.CategoryId,
                           o => o.MapFrom(s => s.CategoryId))
                .ForMember(d => d.CategoryName,
                           o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.ImgUrl,
                           o => o.MapFrom(s => s.ImgUrl))
                .ReverseMap()
                .ForMember(d => d.Category, o => o.Ignore());
            CreateMap<AddAssetDto, Assets>()
                .ForMember(d => d.CategoryId,
                           o => o.MapFrom(s => s.CategoryId))

                .ReverseMap();

        }
    }
}
