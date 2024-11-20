using System.Security.Claims;
using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class RepresentativeService(AppDbContext context, IAdminService adminService, IOrganizationService organizationService, IStudentService studentService): IRepresentativeService
{
    private readonly AppDbContext _context = context;
    private readonly IAdminService _adminService = adminService;
    private readonly IOrganizationService _organizationService = organizationService;
    private readonly IStudentService _studentService = studentService;

    public async Task<List<FieldOfStudyInfoDto>?> GetFieldOfStudyLogToEdit(Guid userId)
    {
        var newestAcademicOrganizationId = _organizationService.GetNewestOrganizationId();
       
        var fieldOfStudyLogs = await _context.FieldOfStudyLogs
            .AsNoTracking()
            .Where(f => f.OrganizationsOfTheYearId == newestAcademicOrganizationId)
            .Include(f => f.FieldOfStudy)
            .Select(f => ConvertDtos.ToFieldOfStudyInfoDto(f))
            .ToListAsync();

        if (_adminService.IsAdmin(userId)) return fieldOfStudyLogs;
        return await _studentService.GetStudentFieldsOfStudy(userId);
    }

    public async Task<bool> IsRepresentative(Guid userId)
    {
        var fieldsOfStudy = await GetFieldOfStudyLogToEdit(userId);
        return fieldsOfStudy != null;
    }

    public async Task<List<ClassDto>> GetClasses(int fieldOfStudyId)
    {
        var classes = await _context.Classes
            .AsNoTracking()
            .Where(c => c.FieldOfStudyLogId == fieldOfStudyId)
            .Include(c => c.EndHour)
            .Include(c => c.StartHour)
            .Include(c=>c.Group)
            .ToListAsync();
        var classesDto = classes.Select(classEntity => new ClassDto
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                EndHour = classEntity.EndHour.HourInterval,
                StartHour = classEntity.StartHour.HourInterval,
                FieldOfStudyLogId = classEntity.FieldOfStudyLogId,
                GroupName = classEntity.Group.Abbr,
                IsOddWeek = classEntity.IsOddWeek,
                Room = classEntity.Room,
                WeekDay = classEntity.WeekDay
            })
            .ToList();
        return classesDto;
    }

    public async Task CreateClass(CreateClassRequestDto classRequestDto)
    {
        var fieldOfStudyLog = await _context.FieldOfStudyLogs.FirstOrDefaultAsync(f => f.Id == classRequestDto.FieldOfStudyLogId);
        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == classRequestDto.GroupId);
        var startHour = await _context.Hours.FirstOrDefaultAsync(h => h.Id == classRequestDto.StartHourId);
        var endHour = await _context.Hours.FirstOrDefaultAsync(h => h.Id == classRequestDto.EndHourId);
        
        if(fieldOfStudyLog == null || group == null || startHour == null || endHour == null)
            throw new ArgumentException("Cannot find field of study, group or hours.");

        _context.Classes.Add(new Class
        {
            Name = classRequestDto.Name,
            Room = classRequestDto.Room,
            IsOddWeek = classRequestDto.IsOddWeek,
            WeekDay = classRequestDto.WeekDay,
            Group = group,
            FieldOfStudyLog = fieldOfStudyLog,
            StartHour = startHour,
            EndHour = endHour
        });
        
        await _context.SaveChangesAsync();
    }

    public async Task UpdateClass(int id, CreateClassRequestDto classRequestDto)
    {
        var fieldOfStudyLog = await _context.FieldOfStudyLogs.FirstOrDefaultAsync(f => f.Id == classRequestDto.FieldOfStudyLogId);
        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == classRequestDto.GroupId);
        var startHour = await _context.Hours.FirstOrDefaultAsync(h => h.Id == classRequestDto.StartHourId);
        var endHour = await _context.Hours.FirstOrDefaultAsync(h => h.Id == classRequestDto.EndHourId);
        var classEntity = await _context.Classes.FirstOrDefaultAsync(c => c.Id == id);
        
        if(fieldOfStudyLog == null || group == null || startHour == null || endHour == null 
           || classEntity == null)
            throw new ArgumentException("Cannot find field of study, group, hours or class.");
        
        classEntity.Name = classRequestDto.Name;
        classEntity.Room = classRequestDto.Room;
        classEntity.IsOddWeek = classRequestDto.IsOddWeek;
        classEntity.WeekDay = classRequestDto.WeekDay;
        classEntity.Group = group;
        classEntity.StartHour = startHour;
        classEntity.EndHour = endHour;
        classEntity.FieldOfStudyLog = fieldOfStudyLog;
        
        _context.Classes.Update(classEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteClass(int id)
    {
        var classEntity = _context.Classes.FirstOrDefault(c => c.Id == id);
        if(classEntity == null)
            throw new ArgumentException("Class not found.");
        _context.Remove(classEntity);
        await _context.SaveChangesAsync();
    }

    public Task<List<AssignmentDto>> GetAssignments(int fieldOfStudyLogId)
    {
        throw new NotImplementedException();
    }

    public Task CreateAssignment(CreateAssignmentRequestDto assignmentRequestDto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAssignment(int id, CreateAssignmentRequestDto assignmentRequestDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAssignment(int id)
    {
        throw new NotImplementedException();
    }
}