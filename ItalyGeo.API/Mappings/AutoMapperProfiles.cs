using AutoMapper;
using ItalyGeo.API.Models.Domain;
using ItalyGeo.API.Models.DTO.Comune;
using ItalyGeo.API.Models.DTO.Province;
using ItalyGeo.API.Models.DTO.Region;
using System.Runtime.InteropServices;

namespace ItalyGeo.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ComuneDto, Comune>().ReverseMap()
            // Maps the property Province to ProvinceDto
                .ForMember(dest => dest.ProvinceDto, opt => opt.MapFrom(src => src.Province));
            CreateMap<AddComuneRequestDto, Comune>().ReverseMap();
            CreateMap<UpdateComuneRequestDto, Comune>().ReverseMap();

            CreateMap<ProvinceDto, Province>().ReverseMap()
                // Maps the property Region to RegionDto
                .ForMember(dest => dest.RegionDto, opt => opt.MapFrom(src => src.Region));
            CreateMap<AddProvinceRequestDto, Province>().ReverseMap();
            CreateMap<UpdateProvinceRequestDto, Province>().ReverseMap();

            CreateMap<RegionDto, Region>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
        }
    }
}
