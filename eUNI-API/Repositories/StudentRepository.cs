using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Entities.Student;
using eUNI_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Repositories;

public class StudentRepository(AppDbContext context): IStudentRepository
{
    private readonly AppDbContext _context = context;
    
    public async Task<int> GetStudentId(Guid userId)
    {
        var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId);
        if (student == null) throw new UnauthorizedAccessException("User is not a student!");
        return student.Id;
    }
    public async Task<List<StudentGroup>?> GetStudentGroups(int fieldOfStudyLogId, int studentId)
    {
        var studentFieldOfStudy = await _context.StudentFieldsOfStudyLogs
            .FirstOrDefaultAsync(f => f.FieldsOfStudyLogId == fieldOfStudyLogId && f.StudentId == studentId);
        if (studentFieldOfStudy == null) return null;
        return _context.StudentGroups.Where(group => group.StudentsFieldsOfStudyLogId == studentFieldOfStudy.Id).ToList();
    }
    
    public async Task<IEnumerable<int>?> GetStudentGroupIds(int fieldOfStudyId, int studentId)
    {
        var studentGroups = await GetStudentGroups(fieldOfStudyId, studentId);
        return studentGroups?.Select(g => g.Id).ToList();
    }

    public async Task<IEnumerable<FieldOfStudyInfoDto>?> GetStudentFieldsOfStudy(int studentId)
    {
        var studentFieldsOfStudyLogs = await _context.StudentFieldsOfStudyLogs
            .Where(f => f.StudentId == studentId)
            .Include(f=>f.FieldsOfStudyLog)
            .ThenInclude(f=>f.FieldOfStudy)
            .ToListAsync();
        
        var fieldOfStudyInfoDto = studentFieldsOfStudyLogs.Select(fieldOfStudy => new FieldOfStudyInfoDto
        {
            FieldOfStudyLogId = fieldOfStudy.FieldsOfStudyLogId,
            Semester = fieldOfStudy.FieldsOfStudyLog.Semester,
            Name = fieldOfStudy.FieldsOfStudyLog.FieldOfStudy.Name,
            StudiesCycle = fieldOfStudy.FieldsOfStudyLog.FieldOfStudy.StudiesCycle,
            GroupIds = GetStudentGroupIds(fieldOfStudy.FieldsOfStudyLogId, studentId).Result
        });
        
        return fieldOfStudyInfoDto;
    }

    public string? GetAlbumNumber(int studentId)
    {
        return _context.Students.FirstOrDefault(s => s.Id == studentId)?.AlbumNumber;
    }
}