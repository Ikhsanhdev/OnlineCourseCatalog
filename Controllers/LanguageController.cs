using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourseCatalog.Data;
using OnlineCourseCatalog.Models;
using OnlineCourseCatalog.Enums;
using Microsoft.AspNetCore.Authorization;
using OnlineCourseCatalog.DTOs;
using OnlineCourseCatalog.Responses;

namespace OnlineCourseCatalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LanguageController : ControllerBase
{
    private readonly AppDbContext _context;

    public LanguageController(AppDbContext context)
    {
        _context = context;
    }

    // ✅ GET: api/Languange
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var languages = await _context.Languages
            .Where(c => c.DeletedAt == null)
            .Select(l => new
            {
                l.Id,
                l.Name
            })
            .ToListAsync();

        if (languages == null)
            return NotFound(new ApiResponse<object>(
                false,
                "Language not found",
                null
            ));

        var response = new ApiResponse<object>(
            true,
            "Courses retrieved successfully",
            languages
        );

        return Ok(response);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var languages = await _context.Languages
            .Where(c => c.Id == id & c.DeletedAt == null)
            .Select(l => new
            {
                l.Id,
                l.Name
            })
            .FirstOrDefaultAsync();

        if (languages == null)
            return NotFound(new ApiResponse<object>(
                false,
                "Language not found",
                null
            ));

        return Ok(new ApiResponse<object>(
            true,
            "languages retrieved successfully",
            languages
        ));
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateLanguageDto dto)
    {
        var language = new Language
        {
            Name = dto.Name,
            CreatedAt = DateTime.Now
        };

        _context.Languages.Add(language);
        await _context.SaveChangesAsync();

        return StatusCode(201, new ApiResponse<object>(
            true,
            "language created successfully",
            new { language.Id }
        ));
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateLanguageDto dto)
    {
        var language = await _context.Languages.FindAsync(id);

        if (language == null)
            return NotFound(new ApiResponse<object>(
                false,
                "Language not found",
                null
            ));

        language.Name = dto.Name;
        language.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return Ok(new ApiResponse<object>(
            true,
            "language updated successfully",
            new { language.Id }
        ));
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var language = await _context.Languages
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (language == null)
            return NotFound(new ApiResponse<object>(
                false,
                "Language not found",
                null
            ));

        if (language.DeletedAt != null)
            return NotFound(new ApiResponse<object>(
                false,
                "Language ready deleted",
                null
            ));

        language.DeletedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return Ok(new ApiResponse<object>(
            true,
            "language deleted successfully",
            null
        ));
    }
}