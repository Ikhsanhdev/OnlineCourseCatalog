using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourseCatalog.Data;
using OnlineCourseCatalog.DTOs;
using OnlineCourseCatalog.Helpers;
using OnlineCourseCatalog.Models;
using OnlineCourseCatalog.Enums;
using OnlineCourseCatalog.Responses;
using Microsoft.AspNetCore.Authorization;

namespace OnlineCourseCatalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(AppDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [Authorize(Roles = "ADMIN")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _context.Users
            .Where(c => c.DeletedAt == null)
            .Select(u => new
            {
                u.Id,
                u.Name,
                u.Email
            })
            .ToListAsync();

        if (users == null)
            return NotFound(new ApiResponse<object>(
                false,
                "user not found",
                null
            ));

        var response = new ApiResponse<object>(
            true,
            "Courses retrieved successfully",
            users
        );

        return Ok(response);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var users = await _context.Users
            .Where(u => u.Id == id & u.DeletedAt == null)
            .Select(u => new
            {
                u.Id,
                u.Name,
                u.Email
            })
            .FirstOrDefaultAsync();

        if (users == null)
            return NotFound(new ApiResponse<object>(
                false,
                "user not found",
                null
            ));

        return Ok(new ApiResponse<object>(
            true,
            "users retrieved successfully",
            users
        ));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (existingUser != null)
            return BadRequest("Email already registered.");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.USER,
            CreatedAt = DateTime.Now
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if(user.DeletedAt != null) 
            return Unauthorized("This user has been deleted, call administrator");

        if (user == null)
            return NotFound(new ApiResponse<object>(
                false,
                "user not found",
                null
            ));

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

        if (!isPasswordValid)
            return Unauthorized("Invalid email or password.");

        var token = _jwtService.GenerateToken(user);

        return Ok(new
        {
            token
        });
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound(new ApiResponse<object>(
                false,
                "user not found",
                null
            ));

        user.Name = dto.Name;
        user.Email = dto.Email;
        user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return Ok(new ApiResponse<object>(
            true,
            "user updated successfully",
            new { user.Id }
        ));
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (user == null)
            return NotFound(new ApiResponse<object>(
                false,
                "user not found",
                null
            ));

        if (user.DeletedAt != null)
            return NotFound(new ApiResponse<object>(
                false,
                "user already deleted",
                null
            ));

        user.DeletedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return Ok(new ApiResponse<object>(
            true,
            "user deleted successfully",
            null
        ));
    }
}