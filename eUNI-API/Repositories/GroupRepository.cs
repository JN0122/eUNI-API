using eUNI_API.Data;
using eUNI_API.Models.Dto.Group;
using eUNI_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Repositories;

public class GroupRepository(AppDbContext context): IGroupRepository
{
    private readonly AppDbContext _context = context;
    public GroupDto GetGroup(int classId)
    {
        var classEntity = _context.Classes.Include(c=>c.Group).FirstOrDefault(c => c.Id == classId);
        if (classEntity == null) throw new ArgumentException($"Class not found: {classId}");
        return new GroupDto
        {
            GroupId = classEntity.GroupId,
            GroupName = classEntity.Group.Abbr,
            Type = classEntity.Group.Type
        };
    }
}