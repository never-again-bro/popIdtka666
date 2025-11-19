using System.ComponentModel.DataAnnotations;
namespace TestingPlatform.Domain.Models;

public class Group
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public int DirectionId { get; set; }
    public Direction Direction { get; set; }
    [Required]
    public int CourseId { get; set; }
    public Course Course { get; set; }
    [Required]
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    public List<Student> Students { get; set; }
    public List<Test> Tests { get; set; }
}

    