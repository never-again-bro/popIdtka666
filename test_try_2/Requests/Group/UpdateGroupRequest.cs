using System.ComponentModel.DataAnnotations;

public class UpdateGroupRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int DirectionId { get; set; }
    public int CourseId { get; set; }
    public int ProjectId { get; set; }
}