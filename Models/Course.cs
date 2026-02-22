using System.ComponentModel.DataAnnotations;
using OnlineCourseCatalog.Enums;

namespace OnlineCourseCatalog.Models;

public class Course
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    public decimal DiscountRate { get; set; } = 0;

    public CourseLevel Level { get; set; }
    public string? ShortDescription { get; set; } = string.Empty;
    public DateTime? CreatedAt {get; set;}
    public DateTime? UpdatedAt {get; set;}
    public DateTime? DeletedAt {get; set;}
    public string? ThumbnailUrl {get; set;}
    // 🔗 Language
    public Guid LanguageId { get; set; }
    public Language Language { get; set; } = null!;

    // 🔗 Topic
    public Guid TopicId { get; set; }
    public Topic Topic { get; set; } = null!;

    // 🔗 Created By
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;
}