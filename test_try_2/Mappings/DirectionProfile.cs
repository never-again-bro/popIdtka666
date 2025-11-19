using AutoMapper;
using practice.Responses.Direction;
using TestingPlatform.Application.Dtos;

namespace TestingPlatform.Mappings;

public class DirectionProfile : Profile
{
    public DirectionProfile()
    {
        CreateMap<DirectionDto, DirectionResponse>();
    }
}