using System.Security.Claims;
using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Group;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class RepresentativeService(AppDbContext context, IFieldOfStudyRepository fieldOfStudyRepository, IAuthRepository authRepository, IOrganizationRepository organizationRepository, IStudentRepository studentRepository, IGroupRepository groupRepository): IRepresentativeService
{
    private readonly AppDbContext _context = context;
    private readonly IFieldOfStudyRepository _fieldOfStudyRepository = fieldOfStudyRepository;
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IOrganizationRepository _organizationRepository = organizationRepository;
    private readonly IStudentRepository _studentRepository = studentRepository;
    private readonly IGroupRepository _groupRepository = groupRepository;

    public async Task<IEnumerable<FieldOfStudyInfoDto>?> FieldOfStudyLogsToEdit(Guid userId)
    {
        var newestAcademicOrganizationId = _organizationRepository.GetNewestOrganizationId();
        if (_authRepository.IsAdmin(userId))
        {
            var fieldOfStudyLogs = await _fieldOfStudyRepository.GetFieldOfStudyLogs(newestAcademicOrganizationId);
            return fieldOfStudyLogs;
        }
        
        var studentId = await _studentRepository.GetStudentId(userId);
        if(studentId == null) return null;
        return _studentRepository.GetRepresentativeFieldsOfStudy(studentId.Value, newestAcademicOrganizationId)!.Select(dto => new FieldOfStudyInfoDto
        {
            FieldOfStudyLogId = dto.FieldOfStudyLogId,
            Name = dto.Name,
            Semester = dto.Semester,
            StudiesCycle = dto.StudiesCycle,
            IsFullTime = dto.IsFullTime
        });
    }
    
    public async Task<IEnumerable<ClassDto>> GetClasses(int fieldOfStudyLogId)
    {
        var classes = await _context.Classes
            .AsNoTracking()
            .Where(c => c.FieldOfStudyLogId == fieldOfStudyLogId)
            .Include(c => c.EndHour)
            .Include(c => c.StartHour)
            .Include(c=>c.Group)
            .ToListAsync();
        var classesDto = classes.Select(classEntity => new ClassDto
            {
                Id = classEntity.Id,
                ClassName = classEntity.Name,
                GroupId = classEntity.GroupId,
                GroupName = _groupRepository.GetGroupName(classEntity.Id),
                EndHour = ConvertDtos.ToHourDto(classEntity.EndHour),
                StartHour = ConvertDtos.ToHourDto(classEntity.StartHour),
                FieldOfStudyLogId = classEntity.FieldOfStudyLogId,
                IsOddWeek = classEntity.IsOddWeek,
                ClassRoom = classEntity.Room,
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

    public async Task<IEnumerable<AssignmentDto>> GetAssignments(int fieldOfStudyLogId)
    {
        var assignments = await _context.Assignments
            .AsNoTracking()
            .Include(a => a.Class)
            .ThenInclude(c => c.Group)
            .Where(a => a.Class.FieldOfStudyLogId == fieldOfStudyLogId)
            .ToListAsync();
        var assignmentsDto = assignments.Select(assignment => new AssignmentDto
            {
                Id = assignment.Id,
                AssignmentName = assignment.Name,
                DeadlineDate = assignment.DeadlineDate,
                ClassId = assignment.Class.Id,
                ClassName = ClassHelper.GetClassWithGroup(assignment.Class.Name, assignment.Class.Group.Abbr),
            })
            .ToList();
        return assignmentsDto;
    }

    public async Task CreateAssignment(CreateAssignmentRequestDto assignmentRequestDto)
    {
        var classEntity = await _context.Classes.FirstOrDefaultAsync(f => f.Id == assignmentRequestDto.ClassId);
        
        if(classEntity == null)
            throw new ArgumentException("Cannot find class for assignment.");
        
        _context.Assignments.Add(new Assignment
        {
            Class = classEntity,
            DeadlineDate = assignmentRequestDto.DeadlineDate,
            Name = assignmentRequestDto.AssignmentName,
        });
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAssignment(int id, CreateAssignmentRequestDto assignmentRequestDto)
    {
        var assignmentEntity = _context.Assignments.FirstOrDefault(a => a.Id == id);
        if (assignmentEntity == null) throw new ArgumentException("Assignment not found.");
        
        var classEntity = _context.Classes.FirstOrDefault(c => c.Id == assignmentRequestDto.ClassId);
        if(classEntity == null) throw new ArgumentException("Class not found.");
        
        assignmentEntity.Name = assignmentRequestDto.AssignmentName;
        assignmentEntity.DeadlineDate = assignmentRequestDto.DeadlineDate;
        assignmentEntity.Class = classEntity;
        
        _context.Assignments.Update(assignmentEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAssignment(int id)
    {
        var assignmentEntity = _context.Assignments.FirstOrDefault(a => a.Id == id);
        if (assignmentEntity == null) throw new ArgumentException("Assignment not found.");
        
        _context.Assignments.Remove(assignmentEntity);
        await _context.SaveChangesAsync();
    }

    public IEnumerable<GroupDto> GetAllGroups(int fieldOfStudyLogId)
    {
        return _studentRepository.GetAllGroups(fieldOfStudyLogId);
    }
}