using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Requests.Answer;
using TestingPlatform.Requests.Student;
using TestingPlatform.Responses.Course;
using TestingPlatform.Responses.Test;

namespace TestingPlatform.Mappings;

public class AnswerProfile : Profile
{
    public AnswerProfile()
    {
        CreateMap<AnswerDto, AnswerResponse>();
        CreateMap<UpdateAnswerRequest, AnswerDto>();
    }
}