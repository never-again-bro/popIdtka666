using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Requests.Answer;

namespace TestingPlatform.Mappings;

public class AnswerProfile : Profile
{
    public AnswerProfile()
    {
        CreateMap<AnswerDto, AnswerResponse>();
        CreateMap<UpdateAnswerRequest, AnswerDto>();
    }
}