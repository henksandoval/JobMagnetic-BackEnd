using AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;

namespace JobMagnet.AutoMapper;

public class MapperProfiles : Profile
{
    public MapperProfiles()
    {
        CreateMap<AboutEntity, AboutModel>().ReverseMap();
        CreateMap<AboutCreateRequest, AboutEntity>().ReverseMap();
    }
}