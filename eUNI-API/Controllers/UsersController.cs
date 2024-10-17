using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eUNI_API.Models.Entities.User;

namespace eUNI_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [Route("Lecturers")]
    public async Task<ActionResult<IEnumerable<LecturerInfo>>> GetLecturers()
    {
        var lecturers = await _context.Users
            .Include(u=>u.Lecturer)
            .ThenInclude(l=>l.EmploymentUnit)
            .ThenInclude(e => e.AcademicDepartment)
            .Where(u=>u.Lecturer != null)
            .Select(u => new LecturerInfo
            {
                Id = u.Id,
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Email = u.Email,
                RoleName = u.Role.Name,
                EmploymentUnit = u.Lecturer.EmploymentUnit.Abbr,
                AcademicDepartment = u.Lecturer.EmploymentUnit.AcademicDepartment.Abbr
            }).ToListAsync();
        return lecturers;
    }
    
    [HttpPost]
    public async Task<IResult> CreateStudent(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        
        return Results.Ok();
    }
}