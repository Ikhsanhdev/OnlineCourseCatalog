using System.ComponentModel.DataAnnotations;
using OnlineCourseCatalog.Enums;

namespace OnlineCourseCatalog.Models;

public class Language
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public DateTime? UpdatedAt {get; set;}
    public DateTime? DeletedAt {get; set;}
    public DateTime? CreatedAt {get; set;}

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}