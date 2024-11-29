using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Repositories;

public class FieldOfStudyRepository(AppDbContext context): IFieldOfStudyRepository
{
    private readonly AppDbContext _context = context;

    public FieldOfStudyLog GetFieldOfStudyLogById(int fieldOfStudyLogId)
    {
        var fieldOfStudyLog = _context.FieldOfStudyLogs.FirstOrDefault(f => f.Id == fieldOfStudyLogId);
        if(fieldOfStudyLog == null) throw new ArgumentException($"Field of study log not found: {fieldOfStudyLogId}");
        return fieldOfStudyLog;
    }

    public async Task<List<FieldOfStudyInfoDto>?> GetFieldOfStudyLogs(int academicOrganizationId)
    {
        var fieldOfStudyLogs = await _context.FieldOfStudyLogs
            .AsNoTracking()
            .Where(f => f.OrganizationsOfTheYearId == academicOrganizationId)
            .Include(f => f.FieldOfStudy)
            .Select(f => ConvertDtos.ToFieldOfStudyInfoDto(f))
            .ToListAsync();

        return fieldOfStudyLogs;
    }

    public List<DayOff> GetDaysOff(int fieldOfStudyLogId)
    {
        var fieldOfStudyLog = _context.FieldOfStudyLogs.FirstOrDefault(f => f.Id == fieldOfStudyLogId);
        if(fieldOfStudyLog == null) throw new ArgumentException($"Field of study log not found: {fieldOfStudyLogId}");
        return _context.DaysOff.Where(d => d.OrganizationsOfTheYearId == fieldOfStudyLog.OrganizationsOfTheYearId)
            .ToList();
    }

    public IEnumerable<FieldOfStudyInfoDto> GetAllFieldOfStudyLogs()
    {
        return _context.FieldOfStudyLogs
            .AsNoTracking()
            .Include(f=>f.FieldOfStudy)
            .Include(f => f.OrganizationsOfTheYear)
            .ThenInclude(o => o.Year)
            .Select(ConvertDtos.ToFieldOfStudyInfoDto);;
    }
}