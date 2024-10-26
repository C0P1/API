namespace API.Helpers;

using API.DataEntities;
using API.DTOs;
using API.Entities;
using AutoMapper;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberResponse>()
        .ForMember(
            d => d.PhotoUrl,
            o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsMain)!.Url));
        CreateMap<Photo, PhotoResponse>();
    }
}