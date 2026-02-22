using System.ComponentModel.DataAnnotations;
using OnlineCourseCatalog.Enums;

namespace OnlineCourseCatalog.DTOs;

public class UpdateLanguageDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
}