using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Exception;
using eUNI_API.Models.Dto.Group;
using eUNI_API.Models.Entities.Student;
using eUNI_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Repositories;

public class GroupRepository(AppDbContext context): IGroupRepository
{
    private readonly AppDbContext _context = context;
    public GroupDto GetGroupByClass(int classId)
    {
        var classEntity = _context.Classes
            .Include(c=>c.Group)
            .FirstOrDefault(c => c.Id == classId);
        if (classEntity == null) throw new HttpNotFoundException($"Class not found: {classId}");
        return new GroupDto
        {
            GroupId = classEntity.GroupId,
            GroupName = GetGroupName(classEntity.Id),
            Type = classEntity.Group.Type
        };
    }
    
    public Group GetGroupById(int groupId)
    {
        var group = _context.Groups.FirstOrDefault(g => g.Id == groupId);
        if(group == null) throw new HttpNotFoundException($"Group not found: {groupId}");
        return group;
    }
    
    public string GetGroupName(int classId)
    {
        var classEntity = _context.Classes.AsNoTracking().Include(c => c.Group)
            .Include(c => c.FieldOfStudyLog)
            .ThenInclude(f => f.FieldOfStudy)
            .FirstOrDefault(c => c.Id == classId); 
        
        if(classEntity == null) throw new HttpNotFoundException($"Class not found: classId={classId}");

        if (classEntity.Group.Type != (int)GroupType.DeanGroup)
            return classEntity.Group.Abbr;
        
        return classEntity.FieldOfStudyLog.FieldOfStudy.Abbr + classEntity.Group.Abbr;
    }

    public string GetGroupName(int fieldOfStudyLogId, Group group)
    {
        if (group.Type != (int)GroupType.DeanGroup)
            return group.Abbr;
        
        var fieldOfStudyLog = _context.FieldOfStudyLogs
            .AsNoTracking()
            .Include(f => f.FieldOfStudy)
            .FirstOrDefault(f => f.Id == fieldOfStudyLogId); 
        if(fieldOfStudyLog == null) throw new HttpNotFoundException($"Field Of Study Log not found: id={fieldOfStudyLogId}");
        return fieldOfStudyLog.FieldOfStudy.Abbr + group.Abbr;
    }
}