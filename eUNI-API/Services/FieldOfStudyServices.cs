using eUNI_API.Data;
using eUNI_API.Models.Dto.Group;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class FieldOfStudyServices(AppDbContext context, IGroupRepository groupRepository): IFieldOfStudyServices
{
    private readonly AppDbContext _context = context;
    private readonly IGroupRepository _groupRepository = groupRepository;
    
    public async Task<IEnumerable<GroupDto>> GetGroups(int fieldOfStudyLogId)
    {
        var classes = await _context.Classes
            .AsNoTracking()
            .Where(c => c.FieldOfStudyLogId == fieldOfStudyLogId)
            .ToListAsync();
        var groups = classes.Select(c => _groupRepository.GetGroupByClass(c.Id)).DistinctBy(g => g.GroupId);
        return groups;
    }
}