using System.ComponentModel.DataAnnotations;

namespace OnlineCourseCatalog.Models;

public class Topic
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;
    public string? Description {get; set;}
    public DateTime? CreatedAt {get; set;}
    public DateTime? UpdatedAt {get; set;}
    public DateTime? DeletedAt {get; set;}
    // 🔁 Self Reference
    public Guid? ParentId { get; set; }
    public Topic? Parent { get; set; }

    public ICollection<Topic> Children { get; set; } = new List<Topic>();

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}