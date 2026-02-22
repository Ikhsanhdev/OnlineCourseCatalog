using System.ComponentModel.DataAnnotations;
using OnlineCourseCatalog.Enums;

namespace OnlineCourseCatalog.DTOs;

public class CreateCourseDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, 100)]
    public decimal DiscountRate { get; set; } = 0;

    [Required]
    public CourseLevel Level { get; set; }

    [Required]
    public Guid LanguageId { get; set; }

    [Required]
    public Guid TopicId { get; set; }
    
    public string? ShortDescription { get; set; } = string.Empty;
    public string? ThumbnailUrl {get; set;}
}