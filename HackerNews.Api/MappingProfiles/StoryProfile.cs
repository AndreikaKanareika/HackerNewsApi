using AutoMapper;
using HackerNews.Api.Helpers;
using HackerNews.Api.Models;

namespace HackerNews.Api.MappingProfiles;

public class StoryProfile : Profile
{
    public StoryProfile()
    {
        CreateMap<Story, StoryResponseDto>()
            .ForMember(d => d.Time, opt => opt.MapFrom(s => DateTimeHelper.UnixTimeStampToDateTime(s.Time)));
    }
}