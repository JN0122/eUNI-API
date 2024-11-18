using eUNI_API.Data;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class RepresentativeService(AppDbContext context, IAuthService authService): IRepresentativeService
{
    private readonly AppDbContext _context = context;
    private readonly IAuthService _authService = authService;

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
        
        if (_authService.IsAdmin(userId)) return fieldOfStudyLogs.ToList();
        
        var studentId = _context.Students
            .FirstOrDefault(s => s.UserId == userId)?.Id;
        var studentFieldsOfStudy = _context.StudentFieldsOfStudyLogs
            .AsNoTracking()
            .Where(sf => sf.StudentId == studentId && fieldOfStudyLogs.Contains(sf.FieldsOfStudyLogId))
            .ToList();
        
        return studentFieldsOfStudy.Select(sf => sf.Id).ToList();
    }
}