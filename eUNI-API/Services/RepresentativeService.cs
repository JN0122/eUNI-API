using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class RepresentativeService(AppDbContext context): IRepresentativeService
{
    private readonly AppDbContext _context = context;

    private bool IsAdmin(Guid userId)
    {
        return _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == userId)?.RoleId == (int)UserRole.Admin;
    }
    
    public async Task<List<int>?> GetFieldOfStudyLogIdsToEdit(Guid userId)
    {
        var yearMaxId = _context.Years.Max(year => year.Id);
        var newestAcademicOrganizationId = _context.OrganizationsOfTheYear.FirstOrDefault(y => y.Id == yearMaxId)?.Id;
        if (newestAcademicOrganizationId == null) return null;

        var fieldOfStudyLogs = await _context.FieldOfStudyLogs
            .AsNoTracking()
            .Where(f => f.OrganizationsOfTheYearId == newestAcademicOrganizationId)
            .Select(f => f.Id)
            .ToListAsync();
        
        if (IsAdmin(userId)) return fieldOfStudyLogs.ToList();
        
        var studentId = _context.Students
            .FirstOrDefault(s => s.UserId == userId)?.Id;
        var studentFieldsOfStudy = _context.StudentFieldsOfStudyLogs
            .AsNoTracking()
            .Where(sf => sf.StudentId == studentId && fieldOfStudyLogs.Contains(sf.FieldsOfStudyLogId))
            .ToList();
        
        return studentFieldsOfStudy.Select(sf => sf.Id).ToList();
    }
    
    public bool IsRepresentative(Guid userId)
    {
        var isAdmin = IsAdmin(userId);
        if (isAdmin) return true;

        var fieldOfStudyLogIds = GetFieldOfStudyLogIdsToEdit(userId).Result;
        return fieldOfStudyLogIds != null && fieldOfStudyLogIds.Count != 0;
    }
}