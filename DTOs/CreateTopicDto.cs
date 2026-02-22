using System.ComponentModel.DataAnnotations;
using OnlineCourseCatalog.Enums;

namespace OnlineCourseCatalog.DTOs;

public class CreateTopicDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }
}