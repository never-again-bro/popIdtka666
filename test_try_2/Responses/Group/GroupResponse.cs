using practice.Responses.Course;
using practice.Responses.Direction;
using practice.Responses.Project;
using TestingPlatform.Responses.Course;

namespace practice.Responses.Group;

public class GroupResponse : BaseResponse
{
    public DirectionResponse Direction { get; set; }
    public CourseResponse Course { get; set; }
    public ProjectResponse Project { get; set; }
}