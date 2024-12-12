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

    public bool IsStudent(Guid userId)
    {
        var studentLog = _context.StudentFieldsOfStudyLogs.FirstOrDefault(sf => sf.UserId == userId);
        return studentLog != null;
    }

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
        var studentFieldOfStudyLog = _context.StudentFieldsOfStudyLogs.FirstOrDefault(sf => sf.Id == studentFieldOfStudyLogId);
        if(studentFieldOfStudyLog == null) throw new ArgumentException($"StudentFieldOfStudyLog not found: id={studentFieldOfStudyLogId}");
        return studentFieldOfStudyLog;
    }

    private StudentGroup GetStudentGroup(int studentGroupId)
    {
        var studentGroup = _context.StudentGroups.FirstOrDefault(sg => sg.Id == studentGroupId);
        if(studentGroup == null) throw new ArgumentException($"Student group not found");
        return studentGroup;
    }
    
    public async Task<List<GroupDto>> GetGroups(int fieldOfStudyLogId, Guid userId)
    {
        var studentFieldOfStudy = await _context.StudentFieldsOfStudyLogs
            .AsNoTracking()
            .Include(sf => sf.FieldsOfStudyLog)
            .ThenInclude(fl => fl.FieldOfStudy)
            .FirstOrDefaultAsync(f => f.FieldsOfStudyLogId == fieldOfStudyLogId && f.UserId == userId);
        if (studentFieldOfStudy == null) return [];

        var groups = _context.StudentGroups
            .AsNoTracking()
            .Where(group => group.StudentsFieldsOfStudyLogId == studentFieldOfStudy.Id)
            .Include(sg => sg.Group);

        if (!groups.Any()) return [];
        
        return groups.Select(sg=> new GroupDto
            {
                GroupId = sg.Group.Id,
                GroupName = GetGroupName(studentFieldOfStudy.FieldsOfStudyLog.FieldOfStudy, sg.Group),
                Type = sg.Group.Type,
            }).ToList();
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

    public async Task<IEnumerable<StudentFieldOfStudyDto>?> GetStudentFieldsOfStudy(Guid userId)
    {
        var studentFieldsOfStudyLogs = await _context.StudentFieldsOfStudyLogs
            .AsNoTracking()
            .Where(f => f.UserId == userId)
            .Include(f=>f.FieldsOfStudyLog)
            .ThenInclude(f=>f.FieldOfStudy)
            .ToListAsync();
        
        var fieldOfStudyInfoDto = studentFieldsOfStudyLogs.Select(fieldOfStudy => new StudentFieldOfStudyDto
        {
            FieldOfStudyLogId = fieldOfStudy.FieldsOfStudyLogId,
            Semester = fieldOfStudy.FieldsOfStudyLog.Semester,
            Name = fieldOfStudy.FieldsOfStudyLog.FieldOfStudy.Name,
            StudiesCycle = fieldOfStudy.FieldsOfStudyLog.FieldOfStudy.StudiesCycle,
            IsRepresentative = IsRepresentativeForFieldOfStudy(fieldOfStudy.FieldsOfStudyLogId, userId),
            Groups = GetGroups(fieldOfStudy.FieldsOfStudyLogId, userId).Result,
            IsFullTime = fieldOfStudy.FieldsOfStudyLog.FieldOfStudy.IsFullTime
        });
        
        return fieldOfStudyInfoDto;
    }

    public bool IsRepresentativeForFieldOfStudy(int fieldsOfStudyLogId, Guid userId)
    {
        var isRepresentative = _context.StudentFieldsOfStudyLogs
            .AsNoTracking()
            .FirstOrDefault(log => log.FieldsOfStudyLogId == fieldsOfStudyLogId && log.UserId == userId)
            ?.IsRepresentative;
        return isRepresentative != null && isRepresentative.Value;
    }

    public bool IsRepresentative(Guid userId)
    {
        if (!IsStudent(userId)) return false; 
        var studentFieldsOfStudy = GetStudentFieldsOfStudy(userId).Result;
        return studentFieldsOfStudy != null && studentFieldsOfStudy.Any(studentFieldOfStudyDto =>
            IsRepresentativeForFieldOfStudy(studentFieldOfStudyDto.FieldOfStudyLogId, userId));
    }

    public IEnumerable<StudentFieldOfStudyDto>? GetRepresentativeFieldsOfStudy(Guid userId)
    {
        var studentFieldsOfStudy = GetStudentFieldsOfStudy(userId).Result;
        return studentFieldsOfStudy?.Where(fieldOfStudyDto =>
            IsRepresentativeForFieldOfStudy(fieldOfStudyDto.FieldOfStudyLogId, userId));
    }

    public StudentFieldsOfStudyLog GetStudentFieldOfStudyLog(int fieldOfStudyLogId, Guid userId)
    {
        var studentFieldOfStudyLog = _context.StudentFieldsOfStudyLogs.FirstOrDefault(sf =>
            sf.FieldsOfStudyLogId == fieldOfStudyLogId && sf.UserId == userId);
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
    
    public async Task UpdateRepresentativeFields(Guid userId, List<int> representativeFieldsOfStudyLogIds)
    {
        var studentFieldsOfStudyLogs =_context.StudentFieldsOfStudyLogs
            .Where(sf => sf.UserId == userId)
            .ToList();

        var representativeFieldsOfStudy = studentFieldsOfStudyLogs
            .Where(sf => sf.IsRepresentative)
            .ToList();
        
        if (representativeFieldsOfStudy.Count == representativeFieldsOfStudyLogIds.Count) return;
        
        if (representativeFieldsOfStudy.Count > representativeFieldsOfStudyLogIds.Count)
        {
            foreach (var studentFieldsOfStudyLog in representativeFieldsOfStudy)
            {
                studentFieldsOfStudyLog.IsRepresentative = representativeFieldsOfStudyLogIds.Any(
                    representativeFieldsOfStudyLogId => studentFieldsOfStudyLog.Id == representativeFieldsOfStudyLogId);
            }
        }
        else
        {
            foreach(var id in representativeFieldsOfStudyLogIds)
            {
                var fieldOfStudy = studentFieldsOfStudyLogs.FirstOrDefault(sf => sf.FieldsOfStudyLogId == id);
                if (fieldOfStudy != null)
                {
                    fieldOfStudy.IsRepresentative = true;
                    continue;
                }
                _context.StudentFieldsOfStudyLogs.Add(new StudentFieldsOfStudyLog
                {
                    FieldsOfStudyLogId = id,
                    UserId = userId,
                    IsRepresentative = true,
                });
            }
        }
        await _context.SaveChangesAsync();
    }
        
    public StudentFieldOfStudyDto? GetStudentCurrentFieldsOfStudy(Guid userId)
    {
        var studentFieldsOfStudyLog = _context.StudentFieldsOfStudyLogs
            .AsNoTracking()
            .Include(f => f.FieldsOfStudyLog)
            .ThenInclude(f => f.FieldOfStudy)
            .FirstOrDefault(f => f.UserId == userId && f.IsCurrentFieldOfStudy);
        
        if(studentFieldsOfStudyLog == null) return null;
        
        return new StudentFieldOfStudyDto
        {
            FieldOfStudyLogId = studentFieldsOfStudyLog.FieldsOfStudyLogId,
            Semester = studentFieldsOfStudyLog.FieldsOfStudyLog.Semester,
            Name = studentFieldsOfStudyLog.FieldsOfStudyLog.FieldOfStudy.Name,
            StudiesCycle = studentFieldsOfStudyLog.FieldsOfStudyLog.FieldOfStudy.StudiesCycle,
            IsRepresentative = IsRepresentativeForFieldOfStudy(studentFieldsOfStudyLog.FieldsOfStudyLogId, userId),
            Groups = GetGroups(studentFieldsOfStudyLog.FieldsOfStudyLogId, userId).Result,
            IsFullTime = studentFieldsOfStudyLog.FieldsOfStudyLog.FieldOfStudy.IsFullTime
        };
    }

    public async Task SetCurrentFieldOfStudy(Guid userId, int fieldOfStudyLogId)
    {
        StudentFieldsOfStudyLog? fieldOfStudy = null;
        foreach (var studentFieldOfStudyLog in _context.StudentFieldsOfStudyLogs.Where(sf => sf.UserId == userId))
        {
            studentFieldOfStudyLog.IsCurrentFieldOfStudy = false;
            if (studentFieldOfStudyLog.FieldsOfStudyLogId == fieldOfStudyLogId) fieldOfStudy = studentFieldOfStudyLog;
        }

        if (fieldOfStudy == null)
        {
            fieldOfStudy = new StudentFieldsOfStudyLog
            {
                FieldsOfStudyLogId = fieldOfStudyLogId,
                UserId = userId,
                IsCurrentFieldOfStudy = true,
            };
            await _context.AddAsync(fieldOfStudy);
            await _context.SaveChangesAsync();
            return;
        }
        
        fieldOfStudy.IsCurrentFieldOfStudy = true;
        _context.Update(fieldOfStudy);
        await _context.SaveChangesAsync();
    }
}