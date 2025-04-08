namespace API.Helpers;

<<<<<<< HEAD
=======
using System.Globalization;
>>>>>>> Parcial04
using API.DataEntities;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberResponse>().ForMember(d => d.Age, o => o.MapFrom(s => s.BirthDay.CalculateAge()))
        .ForMember(
            d => d.PhotoUrl,
            o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsMain)!.Url));
        CreateMap<Photo, PhotoResponse>();
        CreateMap<MemberUpdateRequest, AppUser>();
<<<<<<< HEAD
=======
        CreateMap<RegisterRequest, AppUser>();
        CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s, CultureInfo.InvariantCulture));
>>>>>>> Parcial04
    }
}