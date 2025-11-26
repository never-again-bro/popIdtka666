using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Domain.Models;

namespace TestingPlatform.Infrastructure.Mappings;

public class AttemptProfile : Profile
{
    public AttemptProfile()
    {
        CreateMap<AttemptDto, Attempt>();
    }
}