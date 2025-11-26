using AutoMapper;
using TestingPlatform.Requests.Attempt;
using TestingPlatform.Application.Dtos;

namespace TestingPlatform.Mappings;

public class AttemptProfile : Profile
{
    public AttemptProfile()
    {
        CreateMap<CreateAttemptRequest, AttemptDto>();
        CreateMap<UpdateAttemptRequest, AttemptDto>();
    }
}