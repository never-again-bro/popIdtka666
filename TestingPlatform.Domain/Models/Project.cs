using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.Domain.Models
{
    public class Project
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Group> Groups { get; set; }
        public List<Test> Tests { get; set; }
    }
}
