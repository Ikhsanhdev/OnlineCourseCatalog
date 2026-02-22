using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourseCatalog.Data;
using OnlineCourseCatalog.Models;
using OnlineCourseCatalog.Enums;
using Microsoft.AspNetCore.Authorization;
using OnlineCourseCatalog.DTOs;
using System.Security.Claims;
using OnlineCourseCatalog.Responses;

namespace OnlineCourseCatalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context)
    {
        _context = context;
    }

    // ✅ GET: api/courses
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CourseLevel? level)
    {
        var query = _context.Courses.AsQueryable();

        if (level.HasValue)
        {
            query = query.Where(c => c.Level == level.Value);
        }

        var courses = await query
            .Where(c => c.DeletedAt == null)
            .Select(c => new
            {
                c.Id,
                c.Title,
                c.Description,
                c.Price,
                c.DiscountRate,
                Level = c.Level.ToString(),
                Language = c.Language.Name,
                Topic = c.Topic.Name,
                CreatedBy = c.CreatedBy.Name
            })
            .ToListAsync();

        if (courses == null)
            return NotFound(new ApiResponse<object>(
                false,
                "Course not found",
                null
            ));

        var response = new ApiResponse<object>(
            true,
            "Courses retrieved successfully",
            courses
        );
        return Ok(response);
    }

    // ✅ GET: api/courses/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var course = await _context.Courses
            .Where(c => c.Id == id & c.DeletedAt == null)
            .Select(c => new
            {
                c.Id,
                c.Title,
                c.Description,
                c.Price,
                c.DiscountRate,
                Level = c.Level.ToString(),
                Language = c.Language.Name,
                Topic = c.Topic.Name,
                CreatedBy = c.CreatedBy.Name
            })
            .FirstOrDefaultAsync();

        if (course == null)
            return NotFound(new ApiResponse<object>(
                false,
                "course not found",
                null
            ));

        return Ok(new ApiResponse<object>(
            true,
            "Course retrieved successfully",
            course
        ));
    }

    // ✅ POST: api/courses
    [Authorize(Roles = "ADMIN")]
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateCourseDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var course = new Course
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            DiscountRate = dto.DiscountRate,
            Level = dto.Level,
            LanguageId = dto.LanguageId,
            TopicId = dto.TopicId,
            CreatedById = Guid.Parse(userId!),
            CreatedAt = DateTime.Now,
            ShortDescription = dto.ShortDescription,
            ThumbnailUrl = dto.ThumbnailUrl
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return StatusCode(201, new ApiResponse<object>(
            true,
            "Course created successfully",
            new { course.Id }
        ));
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateCourseDto dto)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
            return NotFound(new ApiResponse<object>(
                false,
                "course not found",
                null
            ));

        var languageExists = await _context.Languages
            .AnyAsync(l => l.Id == dto.LanguageId);

        if (!languageExists)
            return NotFound(new ApiResponse<object>(
                false,
                "language not found",
                null
            ));

        var topicExists = await _context.Topics
            .AnyAsync(t => t.Id == dto.TopicId);

        if (!topicExists)
            return NotFound(new ApiResponse<object>(
                false,
                "topic not found",
                null
            ));

        course.Title = dto.Title;
        course.Description = dto.Description;
        course.Price = dto.Price;
        course.DiscountRate = dto.DiscountRate;
        course.Level = dto.Level;
        course.LanguageId = dto.LanguageId;
        course.TopicId = dto.TopicId;
        course.ShortDescription = dto.ShortDescription;
        course.ThumbnailUrl = dto.ThumbnailUrl;
        course.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return Ok(new ApiResponse<object>(
            true,
            "Course updated successfully",
            new { course.Id }
        ));
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var course = await _context.Courses
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null)
            return NotFound(new ApiResponse<object>(
                false,
                "course not found",
                null
            ));

        if (course.DeletedAt != null)
            return NotFound(new ApiResponse<object>(
                false,
                "course already deleted",
                null
            ));

        course.DeletedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return Ok(new ApiResponse<object>(
            true,
            "course deleted successfully",
            null
        ));
    }
}