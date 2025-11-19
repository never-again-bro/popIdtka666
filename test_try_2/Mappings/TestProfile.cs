using AutoMapper;
using practice.Responses.Test;
using TestingPlatform.Responses.Test;
using TestingPlatform.Requests.Test;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Requests.Student;

namespace TestingPlatform.Mappings;

public class TestProfile : Profile
{
    public TestProfile()
    {
        CreateMap<TestDto, TestResponse>();
        CreateMap<TestDto, TestForManagerResponse>();
        CreateMap<TestDto, TestForStudentResponse>();
        CreateMap<CreateTestRequest, TestDto>()
            .ForMember(d => d.Id, m => m.Ignore())
            .ForMember(d => d.Title, m => m.MapFrom(s => s.Title))
            .ForMember(d => d.Description, m => m.MapFrom(s => s.Description))
            .ForMember(d => d.IsRepeatable, m => m.MapFrom(s => s.IsRepeatable))
            .ForMember(d => d.Type, m => m.MapFrom(s => s.Type))
            .ForMember(d => d.PublishedAt, m => m.MapFrom(s => s.PublishedAt))
            .ForMember(d => d.Deadline, m => m.MapFrom(s => s.Deadline))
            .ForMember(d => d.DurationMinutes, m => m.MapFrom(s => s.DurationMinutes))
            .ForMember(d => d.PassingScore, m => m.MapFrom(s => s.PassingScore))
            .ForMember(d => d.MaxAttempts, m => m.MapFrom(s => s.MaxAttempts))
            .ForMember(d => d.IsPublic, m => m.Ignore())
            .ForMember(d => d.Students, m => m.Ignore())
            .ForMember(d => d.Projects, m => m.Ignore())
            .ForMember(d => d.Courses, m => m.Ignore())
            .ForMember(d => d.Groups, m => m.Ignore())
            .ForMember(d => d.Directions, m => m.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Students = src.Students.Select(s => new StudentDto { Id = s.Id }).ToList();
                dest.Projects = src.Projects.Select(s => new ProjectDto { Id = s.Id }).ToList();
                dest.Courses = src.Courses.Select(s => new CourseDto { Id = s.Id }).ToList();
                dest.Groups = src.Groups.Select(s => new GroupDto { Id = s.Id }).ToList();
                dest.Directions = src.Directions.Select(s => new DirectionDto { Id = s.Id }).ToList();
            });
        CreateMap<UpdateTestRequest, TestDto>()
            .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
            .ForMember(d => d.Title, m => m.MapFrom(s => s.Title))
            .ForMember(d => d.Description, m => m.MapFrom(s => s.Description))
            .ForMember(d => d.IsRepeatable, m => m.MapFrom(s => s.IsRepeatable))
            .ForMember(d => d.Type, m => m.MapFrom(s => s.Type))
            .ForMember(d => d.PublishedAt, m => m.MapFrom(s => s.PublishedAt))
            .ForMember(d => d.Deadline, m => m.MapFrom(s => s.Deadline))
            .ForMember(d => d.DurationMinutes, m => m.MapFrom(s => s.DurationMinutes))
            .ForMember(d => d.PassingScore, m => m.MapFrom(s => s.PassingScore))
            .ForMember(d => d.MaxAttempts, m => m.MapFrom(s => s.MaxAttempts))
            .ForMember(d => d.IsPublic, m => m.Ignore())
            .ForMember(d => d.Students, m => m.Ignore())
            .ForMember(d => d.Projects, m => m.Ignore())
            .ForMember(d => d.Courses, m => m.Ignore())
            .ForMember(d => d.Groups, m => m.Ignore())
            .ForMember(d => d.Directions, m => m.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Students = src.Students.Select(s => new StudentDto { Id = s.Id }).ToList();
                dest.Projects = src.Projects.Select(s => new ProjectDto { Id = s.Id }).ToList();
                dest.Courses = src.Courses.Select(s => new CourseDto { Id = s.Id }).ToList();
                dest.Groups = src.Groups.Select(s => new GroupDto { Id = s.Id }).ToList();
                dest.Directions = src.Directions.Select(s => new DirectionDto { Id = s.Id }).ToList();
            });
    }
}