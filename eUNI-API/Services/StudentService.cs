using eUNI_API.Data;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.Student;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class StudentService(AppDbContext context): IStudentService
{
    private readonly AppDbContext _context = context;

    private async Task<int> GetStudentId(Guid userId)
    {
        var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId);
        if (student == null) throw new UnauthorizedAccessException("User is not a student!");
        return student.Id;
    }
    private async Task<List<StudentGroup>?> GetStudentGroups(int fieldOfStudyId, Guid userId)
    {
        var studentId = await GetStudentId(userId);
        var studentFieldOfStudy = await _context.StudentFieldsOfStudyLogs
            .FirstOrDefaultAsync(f => f.FieldsOfStudyLogId == fieldOfStudyId && f.StudentId == studentId);
        
        if (studentFieldOfStudy == null) return null;
        
        return _context.StudentGroups.Where(group => group.StudentsFieldsOfStudyLogId == studentFieldOfStudy.Id).ToList();
    }
    
    public async Task<List<int>?> GetStudentGroupIds(int fieldOfStudyId, Guid userId)
    {
        var studentGroups = await GetStudentGroups(fieldOfStudyId, userId);
        return studentGroups?.Select(g => g.Id).ToList();
    }
    
    

    public async Task<List<FieldOfStudyInfoDto>?> GetStudentFieldsOfStudy(Guid userId)
    {
        var studentId = await GetStudentId(userId);
        var studentFieldsOfStudyLogs = await _context.StudentFieldsOfStudyLogs
            .Where(f => f.StudentId == studentId)
            .Include(f=>f.FieldsOfStudyLog)
            .Select(f=>f.FieldsOfStudyLog)
            .ToListAsync();

        var fieldsOfStudy = _context.FieldOfStudies.ToList();
        
        return studentFieldsOfStudyLogs.Select(sfl =>
        {
            var fieldOfStudyInfo = fieldsOfStudy.Find(f => sfl.Id == f.Id);
            if(fieldOfStudyInfo == null) throw new KeyNotFoundException("Field of study info not found!");
            return new FieldOfStudyInfoDto
            {
                FieldOfStudyLogId = sfl.Id,
                Semester = sfl.Semester,
                Name = fieldOfStudyInfo.Name,
                StudiesCycle = fieldOfStudyInfo.StudiesCycle
            };
        }).ToList();
    }

    public List<int>? GetRepresentativeFieldOfStudyLogId(Guid userId)
    {
        var yearMaxId = _context.Years.Max(year => year.Id);
        var newestAcademicOrganizationId = _context.OrganizationsOfTheYear.FirstOrDefault(y => y.Id == yearMaxId)?.Id;
        if (newestAcademicOrganizationId == null) return null;
        
        var fieldOfStudyLogs = _context.FieldOfStudyLogs
            .AsNoTracking()
            .Where(f => f.OrganizationsOfTheYearId == newestAcademicOrganizationId)
            .Select(f => f.Id)
            .ToList();
        
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
        var fieldOfStudyLogIds = GetRepresentativeFieldOfStudyLogId(userId);
        return fieldOfStudyLogIds != null && fieldOfStudyLogIds.Count != 0;
    }
}