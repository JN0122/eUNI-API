using eUNI_API.Data;
using eUNI_API.Exception;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Group;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class FieldOfStudyService(AppDbContext context, IGroupRepository groupRepository, 
    IFieldOfStudyRepository fieldOfStudyRepository, IOrganizationRepository organizationRepository): IFieldOfStudyService
{
    private readonly AppDbContext _context = context;
    private readonly IGroupRepository _groupRepository = groupRepository;
    private readonly IFieldOfStudyRepository _fieldOfStudyRepository = fieldOfStudyRepository;
    private readonly IOrganizationRepository _organizationRepository = organizationRepository;

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

    public async Task<FieldOfStudy> CreateFieldOfStudy(CreateFieldOfStudyRequest createFieldOfStudyRequest)
    {
        return await _fieldOfStudyRepository.CreateFieldOfStudy(createFieldOfStudyRequest);
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

    public async Task<FieldOfStudyLog> CreateFieldOfStudyLog(CreateFieldOfStudyLogRequest createFieldOfStudyLogRequest)
    {
        var fieldOfStudy = _fieldOfStudyRepository.GetFieldOfStudy(createFieldOfStudyLogRequest.FieldOfStudyId);
        var organization = _organizationRepository.GetOrganizationOfTheYear(createFieldOfStudyLogRequest.OrganizationId);

        return await _fieldOfStudyRepository.CreateFieldOfStudyLog(fieldOfStudy, organization, createFieldOfStudyLogRequest.CurrentSemester);
    }

    public async Task DeleteFieldOfStudyLog(int id)
    {
        await _fieldOfStudyRepository.DeleteFieldOfStudyLog(id);
    }

    public async Task UpgradeFieldOfStudyLogs(UpgradeFieldsOfStudyLogsRequest upgradeFieldsOfStudyLogsRequest)
    {
        var organizationToUpgrade = await _organizationRepository.GetOrganizationToUpgrade();
        var newOrganization = await _organizationRepository.GetNewestOrganization();
        var fieldsOfStudyLogs = await _fieldOfStudyRepository.GetFieldsOfStudyLogs();
        var fieldsToUpgrade = fieldsOfStudyLogs
            .Where(f => upgradeFieldsOfStudyLogsRequest.FieldOfStudyLogIds.Contains(f.Id))
            .ToList();

        if (fieldsToUpgrade.Count != upgradeFieldsOfStudyLogsRequest.FieldOfStudyLogIds.Count())
            throw new HttpNotFoundException("Some fields are not in the database.");

        if (fieldsToUpgrade.All(fieldOfStudyLog => fieldOfStudyLog.OrganizationsOfTheYear == organizationToUpgrade))
            throw new HttpBadRequestException("Some fields cannot be upgraded!");

        var newFieldsLogs = new List<FieldOfStudyLog>();
        fieldsToUpgrade.ForEach(field =>
        {
            var newSemester = (byte)(field.Semester + 1);

            var isFieldUpgraded = fieldsOfStudyLogs.FirstOrDefault(fl =>
                fl.OrganizationsOfTheYearId == newOrganization.Id &&
                fl.Semester == newSemester &&
                fl.FieldOfStudyId == field.FieldOfStudyId) != null;

            if (field.FieldOfStudy.SemesterCount < newSemester || isFieldUpgraded)
                return;

            newFieldsLogs.Add(new FieldOfStudyLog
            {
                OrganizationsOfTheYearId = newOrganization.Id,
                Semester = newSemester,
                FieldOfStudyId = field.FieldOfStudyId,
            });
        });
        
        await _context.AddRangeAsync(newFieldsLogs!);
        await _context.SaveChangesAsync();
    }
}