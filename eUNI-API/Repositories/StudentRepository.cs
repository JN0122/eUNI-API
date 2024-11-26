using eUNI_API.Data;
using eUNI_API.Models.Dto.Group;
using eUNI_API.Models.Dto.Student;
using eUNI_API.Models.Entities.Student;
using eUNI_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Repositories;

public class StudentRepository(AppDbContext context): IStudentRepository
{
    private readonly AppDbContext _context = context;
    
    public async Task<int?> GetStudentId(Guid userId)
    {
        var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId);
        return student?.Id;
    }
    public async Task<List<GroupDto>?> GetStudentGroups(int fieldOfStudyLogId, int studentId)
    {
        var studentFieldOfStudy = await _context.StudentFieldsOfStudyLogs
            .FirstOrDefaultAsync(f => f.FieldsOfStudyLogId == fieldOfStudyLogId && f.StudentId == studentId);
        if (studentFieldOfStudy == null) return null;
        
        return _context.StudentGroups
            .Where(group => group.StudentsFieldsOfStudyLogId == studentFieldOfStudy.Id)
            .Include(sg => sg.Group)
            .Select(g=> new GroupDto
            {
                GroupId = g.GroupId,
                GroupName = g.Group.Abbr,
                Type = g.Group.Type,
            }).ToList();
    }

    public async Task<IEnumerable<StudentFieldOfStudyDto>?> GetStudentFieldsOfStudy(int studentId, int academicOrganizationId)
    {
        var studentFieldsOfStudyLogs = await _context.StudentFieldsOfStudyLogs
            .Where(f => f.StudentId == studentId)
            .Include(f=>f.FieldsOfStudyLog)
            .ThenInclude(f=>f.FieldOfStudy)
            .ToListAsync();
        
        var fieldOfStudyInfoDto = studentFieldsOfStudyLogs.Select(fieldOfStudy => new StudentFieldOfStudyDto
        {
            FieldOfStudyLogId = fieldOfStudy.FieldsOfStudyLogId,
            Semester = fieldOfStudy.FieldsOfStudyLog.Semester,
            Name = fieldOfStudy.FieldsOfStudyLog.FieldOfStudy.Name,
            StudiesCycle = fieldOfStudy.FieldsOfStudyLog.FieldOfStudy.StudiesCycle,
            IsRepresentative = IsRepresentativeForFieldOfStudy(fieldOfStudy.FieldsOfStudyLogId, studentId),
            Groups = GetStudentGroups(fieldOfStudy.FieldsOfStudyLogId, studentId).Result
        });
        
        return fieldOfStudyInfoDto;
    }

    public string? GetAlbumNumber(int studentId)
    {
        return _context.Students.FirstOrDefault(s => s.Id == studentId)?.AlbumNumber;
    }

    public bool IsRepresentativeForFieldOfStudy(int fieldsOfStudyLogId, int studentId)
    {
        var isRepresentative = _context.StudentFieldsOfStudyLogs
            .FirstOrDefault(log => log.FieldsOfStudyLogId == fieldsOfStudyLogId && log.StudentId == studentId)
            ?.IsRepresentative;
        return isRepresentative != null && isRepresentative.Value;
    }

    public bool IsRepresentative(Guid userId, int academicOrganizationId)
    {
        var studentId = GetStudentId(userId).Result;
        if (studentId == null) return false; 
        var studentFieldsOfStudy = GetStudentFieldsOfStudy(studentId.Value, academicOrganizationId).Result;
        return studentFieldsOfStudy != null && studentFieldsOfStudy.Any(studentFieldOfStudyDto =>
            IsRepresentativeForFieldOfStudy(studentFieldOfStudyDto.FieldOfStudyLogId, studentId.Value));
    }

    public IEnumerable<StudentFieldOfStudyDto>? GetRepresentativeFieldsOfStudy(int studentId, int academicOrganizationId)
    {
        var studentFieldsOfStudy = GetStudentFieldsOfStudy(studentId, academicOrganizationId).Result;
        return studentFieldsOfStudy?.Where(fieldOfStudyDto =>
            IsRepresentativeForFieldOfStudy(fieldOfStudyDto.FieldOfStudyLogId, studentId));
    }
}