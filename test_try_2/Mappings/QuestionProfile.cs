using AutoMapper;
using practice.Requests.Question;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Requests.Question;
using TestingPlatform.Responses.Question;

namespace practice.Mappings;

public class QuestionProfile : Profile
{
    public QuestionProfile()
    {
        CreateMap<QuestionDto, QuestionResponse>();
        CreateMap<CreateQuestionRequest, QuestionDto>();
        CreateMap<UpdateQuestionRequest, QuestionDto>();
    }
}