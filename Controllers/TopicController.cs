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
public class TopicController : ControllerBase
{
    private readonly AppDbContext _context;

    public TopicController(AppDbContext context)
    {
        _context = context;
    }

    // ✅ GET: api/Languange
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var topics = await _context.Topics
            .Where(c => c.DeletedAt == null)
            .Select(t => new
            {
                t.Id,
                t.Name
            })
            .ToListAsync();

        if (topics == null)
            return NotFound(new ApiResponse<object>(
                false,
                "topic not found",
                null
            ));
            
        var response = new ApiResponse<object>(
            true,
            "Courses retrieved successfully",
            topics
        );

        return Ok(response);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var topics = await _context.Topics
            .Where(c => c.Id == id & c.DeletedAt == null)
            .Select(t => new
            {
                t.Id,
                t.Name
            })
            .FirstOrDefaultAsync();

        if (topics == null)
            return NotFound(new ApiResponse<object>(
                false,
                "topic not found",
                null
            ));

        return Ok(new ApiResponse<object>(
            true,
            "topics retrieved successfully",
            topics
        ));
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateTopicDto dto)
    {
        var topic = new Topic
        {
            Name = dto.Name,
            Description = dto.Description,
            ParentId = dto.ParentId,
            CreatedAt = DateTime.Now
        };

        _context.Topics.Add(topic);
        await _context.SaveChangesAsync();

        return StatusCode(201, new ApiResponse<object>(
            true,
            "topic created successfully",
            new { topic.Id }
        ));
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateTopicDto dto)
    {
        var topic = await _context.Topics.FindAsync(id);

        if (topic == null)
            return NotFound(new ApiResponse<object>(
                false,
                "topic not found",
                null
            ));

        topic.Name = dto.Name;
        topic.Description = dto.Description;
        topic.ParentId = dto.ParentId;
        topic.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return Ok(new ApiResponse<object>(
            true,
            "topic updated successfully",
            new { topic.Id }
        ));
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var topic = await _context.Topics
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (topic == null)
            return NotFound(new ApiResponse<object>(
                false,
                "topic not found",
                null
            ));

        if (topic.DeletedAt != null)
            return NotFound(new ApiResponse<object>(
                false,
                "topic already deleted",
                null
            ));

        topic.DeletedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return Ok(new ApiResponse<object>(
            true,
            "topic deleted successfully",
            null
        ));
    }
}