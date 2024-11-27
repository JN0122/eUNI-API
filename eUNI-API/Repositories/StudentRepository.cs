using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Models.Dto.Group;
using eUNI_API.Models.Dto.Student;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.Student;
using eUNI_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Repositories;

public class StudentRepository(AppDbContext context): IStudentRepository
{
    private readonly AppDbContext _context = context;

    private FieldOfStudy GetFieldOfStudy(int fieldOfStudyLogId)
    {
        var fieldOfStudy = _context.FieldOfStudyLogs
            .AsNoTracking()
            .Include(f=>f.FieldOfStudy)
            .First(f => f.Id == fieldOfStudyLogId);
        if(fieldOfStudy == null) throw new ArgumentException("Group not found");
        return fieldOfStudy.FieldOfStudy;
    }

    private Group GetGroup(int groupId)
    {
        var group = _context.Groups.FirstOrDefault(g => g.Id == groupId);
        if(group == null) throw new ArgumentException("Group not found");
        return group;
    }
    
    private StudentFieldsOfStudyLog GetStudentFieldOfStudyLog(int studentFieldOfStudyLogId)
    {
        var studentFieldOfStudyLog = _context.StudentFieldsOfStudyLogs.FirstOrDefault(sf => sf.FieldsOfStudyLogId == studentFieldOfStudyLogId);
        if(studentFieldOfStudyLog == null) throw new ArgumentException("StudentFieldOfStudyLog not found");
        return studentFieldOfStudyLog;
    }

    private StudentGroup GetStudentGroup(int studentGroupId)
    {
        var studentGroup = _context.StudentGroups.FirstOrDefault(sg => sg.Id == studentGroupId);
        if(studentGroup == null) throw new ArgumentException($"Student group not found");
        return studentGroup;
    }
    
    public async Task<int?> GetStudentId(Guid userId)
    {
        var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId);
        return student?.Id;
    }
    public async Task<IEnumerable<GroupDto>?> GetGroups(int fieldOfStudyLogId, int studentId)
    {
        var studentFieldOfStudy = await _context.StudentFieldsOfStudyLogs
            .FirstOrDefaultAsync(f => f.FieldsOfStudyLogId == fieldOfStudyLogId && f.StudentId == studentId);
        if (studentFieldOfStudy == null) return null;
        
        return _context.StudentGroups
            .Where(group => group.StudentsFieldsOfStudyLogId == studentFieldOfStudy.Id)
            .Include(sg => sg.Group)
            .Select(sg=> new GroupDto
            {
                GroupId = sg.Group.Id,
                GroupName = GetGroupName(studentFieldOfStudy.FieldsOfStudyLog.FieldOfStudy, sg.Group),
                Type = sg.Group.Type,
            });
    }

    private static string GetGroupName(FieldOfStudy fieldOfStudy, Group group)
    {
        if (group.Type != (int)GroupType.DeanGroup)
            return group.Abbr;
        
        return fieldOfStudy.Abbr + group.Abbr;
    }

    public IEnumerable<GroupDto> GetAllGroups(int fieldOfStudyLogId)
    {
        var fieldOfStudy = GetFieldOfStudy(fieldOfStudyLogId);
        return _context.Groups.Select(g => new GroupDto
        {
            GroupId = g.Id,
            GroupName = GetGroupName(fieldOfStudy, g),
            Type = g.Type,
        });
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
            Groups = GetGroups(fieldOfStudy.FieldsOfStudyLogId, studentId).Result,
            IsFullTime = fieldOfStudy.FieldsOfStudyLog.FieldOfStudy.IsFullTime
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

    public StudentFieldsOfStudyLog GetStudentFieldOfStudyLog(int fieldOfStudyLogId, int studentId)
    {
        var studentFieldOfStudyLog = _context.StudentFieldsOfStudyLogs.FirstOrDefault(sf =>
            sf.FieldsOfStudyLogId == fieldOfStudyLogId && sf.StudentId == studentId);
        if(studentFieldOfStudyLog == null) throw new ArgumentException("Student field of study log not found");
        return studentFieldOfStudyLog;
    }

    public StudentGroup? GetStudentGroup(int studentFieldOfStudyLogId, int groupType)
    {
        return _context.StudentGroups
            .Include(sg => sg.Group)
            .FirstOrDefault(sg => sg.StudentsFieldsOfStudyLogId == studentFieldOfStudyLogId && sg.Group.Type == groupType);
    }

    public void JoinGroup(int studentFieldOfStudyLogId, int groupId)
    {
        _context.StudentGroups.Add(new StudentGroup
        {
            Group = GetGroup(groupId),
            StudentsFieldsOfStudyLog = GetStudentFieldOfStudyLog(studentFieldOfStudyLogId)
        });
        _context.SaveChanges();
    }

    public void ChangeGroup(int studentGroupId, int groupId)
    {
        var studentGroup = GetStudentGroup(studentGroupId);
        studentGroup.Group = GetGroup(groupId);
        _context.Update(studentGroup);
        _context.SaveChanges();
    }
}