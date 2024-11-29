using System.Security.Claims;
using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Group;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class RepresentativeService(AppDbContext context, 
    IFieldOfStudyRepository fieldOfStudyRepository, IAuthRepository authRepository, 
    IOrganizationRepository organizationRepository, IStudentRepository studentRepository, 
    IGroupRepository groupRepository, IHourRepository hourRepository,
    IClassesRepository classesRepository): IRepresentativeService
{
    private readonly AppDbContext _context = context;
    private readonly IFieldOfStudyRepository _fieldOfStudyRepository = fieldOfStudyRepository;
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IOrganizationRepository _organizationRepository = organizationRepository;
    private readonly IStudentRepository _studentRepository = studentRepository;
    private readonly IGroupRepository _groupRepository = groupRepository;
    private readonly IHourRepository _hourRepository = hourRepository;
    private readonly IClassesRepository _classesRepository = classesRepository;

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
        var classes = _classesRepository.GetClasses(fieldOfStudyLogId)
            .Include(c=>c.EndHour)
            .Include(c=>c.StartHour)
            .Include(c=>c.ClassDates)
            .ToList();

        return classes.Select(classEntity => new ClassDto
            {
                Id = classEntity.Id,
                ClassName = classEntity.Name,
                GroupId = classEntity.GroupId,
                GroupName = _groupRepository.GetGroupName(classEntity.Id),
                EndHour = ConvertDtos.ToHourDto(classEntity.EndHour),
                StartHour = ConvertDtos.ToHourDto(classEntity.StartHour),
                FieldOfStudyLogId = classEntity.FieldOfStudyLogId,
                ClassRoom = classEntity.Room,
                Dates = classEntity.ClassDates.Select(cd => cd.Date)
            })
            .ToList();
    }
    
    private List<DateOnly> CalculateClassDate(OrganizationOfTheYear organizationInfo, bool? isOddWeek, WeekDay weekDay)
    {
        var repeatClassInDays = isOddWeek == null ? 7 : 14;
        var startFirstWeek = isOddWeek ?? true;

        var dates = DateHelper.CalculateDates(
            organizationInfo.StartDay, organizationInfo.EndDay, weekDay, 
            repeatClassInDays, startFirstWeek
        );
        
        var daysOff = _organizationRepository.GetDaysOff(organizationInfo.Id).Result;
        return dates.Where(d => !daysOff.Contains(d)).ToList();
    }

    public async Task CreateClass(CreateClassRequestDto classRequestDto)
    {
        var fieldOfStudyLog = _fieldOfStudyRepository.GetFieldOfStudyLogById(classRequestDto.FieldOfStudyLogId);
        var group = _groupRepository.GetGroupById(classRequestDto.GroupId);
        var startHour = _hourRepository.GetHourById(classRequestDto.StartHourId);
        var endHour = _hourRepository.GetHourById(classRequestDto.EndHourId);
        var organization = _organizationRepository.GetOrganizationsInfo(classRequestDto.FieldOfStudyLogId).Result;
        
        var classEntity = _context.Classes.Add(new Class
        {
            Name = classRequestDto.Name,
            Room = classRequestDto.Room,
            Group = group,
            FieldOfStudyLog = fieldOfStudyLog,
            StartHour = startHour,
            EndHour = endHour
        }).Entity;
        
        var dates = CalculateClassDate(organization, classRequestDto.IsOddWeek, classRequestDto.WeekDay);
        if (dates.Count == 0) throw new ArgumentException("Cannot create class with no dates, please check if you selected a valid days.");
        
        _context.ClassDates.AddRange(dates.Select(d=>new ClassDate
        {
            Class = classEntity,
            Date = d
        }));
        
        await _context.SaveChangesAsync();
    }

    private void UpdateClassDates(int classId, List<DateOnly> dates)
    {
        var classDates = _classesRepository.GetClassDates(classId).ToList();
        if(classDates.Count == 0) throw new Exception("Use CreateClassDates function");
        var entityDifference = dates.Count - classDates.Count;
        
        switch (entityDifference)
        {
            case > 0:
                var classDatesToAdd = new List<ClassDate>();
                for(var i = 0; i < entityDifference; i++) classDatesToAdd.Add(new ClassDate{ClassId = classId});
                _context.ClassDates.AddRange(classDatesToAdd);
                break;
            case < 0:
                var classDatesToRemove = classDates.Slice(classDates.Count + entityDifference - 1, -entityDifference);
                _context.ClassDates.RemoveRange(classDatesToRemove);
                break;
        }

        if (entityDifference != 0)
        {
            _context.SaveChanges();
            classDates = _classesRepository.GetClassDates(classId).ToList();
        }
        
        var newClassDates = classDates.Select((classDate, index) =>
        {
            classDate.Date = dates[index];
            return classDate;
        });
        _context.ClassDates.UpdateRange(newClassDates);
        _context.SaveChanges();
    }

    public async Task UpdateClass(int id, UpdateClassRequestDto updateClassRequestDto)
    {
        var fieldOfStudyLog = _fieldOfStudyRepository.GetFieldOfStudyLogById(updateClassRequestDto.FieldOfStudyLogId);
        var group = _groupRepository.GetGroupById(updateClassRequestDto.GroupId);
        var startHour = _hourRepository.GetHourById(updateClassRequestDto.StartHourId);
        var endHour = _hourRepository.GetHourById(updateClassRequestDto.EndHourId);
        var classEntity = _classesRepository.GetClassById(id);
        
        classEntity.Name = updateClassRequestDto.Name;
        classEntity.Room = updateClassRequestDto.Room;
        classEntity.Group = group;
        classEntity.StartHour = startHour;
        classEntity.EndHour = endHour;
        classEntity.FieldOfStudyLog = fieldOfStudyLog;
        
        UpdateClassDates(classEntity.Id, updateClassRequestDto.Dates.ToList());
        
        _context.Classes.Update(classEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteClass(int id)
    {
        var classEntity = _classesRepository.GetClassById(id);
        var classDates = _classesRepository.GetClassDates(classEntity.Id);
        _context.RemoveRange(classDates);
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