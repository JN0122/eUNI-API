using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Group;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class FieldOfStudyService(AppDbContext context, IGroupRepository groupRepository, 
    IFieldOfStudyRepository fieldOfStudyRepository): IFieldOfStudyService
{
    private readonly AppDbContext _context = context;
    private readonly IGroupRepository _groupRepository = groupRepository;
    private readonly IFieldOfStudyRepository _fieldOfStudyRepository = fieldOfStudyRepository;

    public async Task<IEnumerable<FieldOfStudyDto>> GetFieldsOfStudy()
    {
        var fields = await _fieldOfStudyRepository.GetFieldsOfStudy();
        
        return fields.Select(field => new FieldOfStudyDto
        {
            Id = field.Id,
            Abbr = field.Abbr,
            FullTime = field.IsFullTime,
            Name = field.Name,
            SemesterCount = field.SemesterCount,
            StudiesCycle = field.StudiesCycle,
        });
    }

    public async Task CreateFieldOfStudy(CreateFieldOfStudyRequest createFieldOfStudyRequest)
    {
        await _fieldOfStudyRepository.CreateFieldOfStudy(createFieldOfStudyRequest);
    }

    public async Task UpdateFieldOfStudy(int id, CreateFieldOfStudyRequest createFieldOfStudyRequest)
    {
        await _fieldOfStudyRepository.UpdateFieldOfStudy(id, createFieldOfStudyRequest);
    }

    public async Task DeleteFieldOfStudy(int id)
    {
        await _fieldOfStudyRepository.DeleteFieldOfStudy(id);
    }

    public async Task<IEnumerable<GroupDto>> GetGroups(int fieldOfStudyLogId)
    {
        var classes = await _context.Classes
            .AsNoTracking()
            .Where(c => c.FieldOfStudyLogId == fieldOfStudyLogId)
            .ToListAsync();
        var groups = classes.Select(c => _groupRepository.GetGroupByClass(c.Id)).DistinctBy(g => g.GroupId);
        return groups;
    }

    public async Task<List<FieldOfStudyInfoDto>> GetFieldsOfStudyLogsInfoDtos()
    {
        var fieldsOfStudy = await _fieldOfStudyRepository.GetFieldsOfStudyLogs();
        return fieldsOfStudy.Select(ConvertDtos.ToFieldOfStudyInfoDto).ToList();
    }
}