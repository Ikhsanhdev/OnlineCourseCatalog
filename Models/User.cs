using System.ComponentModel.DataAnnotations;
using OnlineCourseCatalog.Enums;

namespace OnlineCourseCatalog.Models;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public DateTime? CreatedAt {get; set;}
    public DateTime? UpdatedAt {get; set;}
    public DateTime? DeletedAt {get; set;}

    public UserRole Role { get; set; } = UserRole.USER;

    // 🔗 Relation
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}