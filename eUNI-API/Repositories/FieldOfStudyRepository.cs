using eUNI_API.Data;
using eUNI_API.Exception;
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

    private FieldOfStudy GetFieldOfStudyById(int id)
    {
        var field = _context.FieldOfStudies.AsNoTracking().FirstOrDefault(f => f.Id == id);
        if(field == null) throw new HttpNotFoundException($"Field of study with id {id} does not exist");
        return field;
    }

    public async Task<List<FieldOfStudy>> GetFieldsOfStudy()
    {
        return await _context.FieldOfStudies.ToListAsync();
    }

    public async Task CreateFieldOfStudy(CreateFieldOfStudyRequest createFieldOfStudyRequest)
    {
        _context.Add(new FieldOfStudy
        {
            Name = createFieldOfStudyRequest.Name,
            Abbr = createFieldOfStudyRequest.Abbr,
            StudiesCycle = createFieldOfStudyRequest.StudiesCycle,
            SemesterCount = createFieldOfStudyRequest.SemesterCount,
            IsFullTime = createFieldOfStudyRequest.FullTime
        });
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFieldOfStudy(int id, CreateFieldOfStudyRequest createFieldOfStudyRequest)
    {
        var fieldOfStudy = GetFieldOfStudyById(id);
        fieldOfStudy.Name = createFieldOfStudyRequest.Name;
        fieldOfStudy.Abbr = createFieldOfStudyRequest.Abbr;
        fieldOfStudy.StudiesCycle = createFieldOfStudyRequest.StudiesCycle;
        fieldOfStudy.SemesterCount = createFieldOfStudyRequest.SemesterCount;
        fieldOfStudy.IsFullTime = createFieldOfStudyRequest.FullTime;
        _context.Update(fieldOfStudy);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFieldOfStudy(int id)
    {
        var fieldOfStudy = GetFieldOfStudyById(id);
        _context.Remove(fieldOfStudy);
        await _context.SaveChangesAsync();
    }

    public void DeleteAllFieldsOfStudy()
    {
        _context.FieldOfStudies.RemoveRange(_context.FieldOfStudies);
        _context.SaveChanges();
    }

    public FieldOfStudyLog GetFieldOfStudyLogById(int fieldOfStudyLogId)
    {
        var fieldOfStudyLog = _context.FieldOfStudyLogs.FirstOrDefault(f => f.Id == fieldOfStudyLogId);
        if(fieldOfStudyLog == null) throw new HttpNotFoundException($"Field of study log not found: {fieldOfStudyLogId}");
        return fieldOfStudyLog;
    }

    public FieldOfStudyInfoDto GetFieldOfStudyInfo(int fieldOfStudyLogId)
    {
        var fieldOfStudyLog = _context.FieldOfStudyLogs
            .AsNoTracking()
            .Include(f=>f.FieldOfStudy)
            .Include(f => f.OrganizationsOfTheYear)
            .ThenInclude(o => o.Year)
            .FirstOrDefault(f=>f.Id == fieldOfStudyLogId);
        if(fieldOfStudyLog == null) throw new HttpNotFoundException($"Field of study log not found: {fieldOfStudyLogId}");
        return ConvertDtos.ToFieldOfStudyInfoDto(fieldOfStudyLog);
    }

    public async Task<List<FieldOfStudyInfoDto>?> GetFieldOfStudyLogs(int academicOrganizationId)
    {
        var fieldOfStudyLogs = _context.FieldOfStudyLogs
            .AsNoTracking()
            .Where(f => f.OrganizationsOfTheYearId == academicOrganizationId)
            .Include(f=>f.FieldOfStudy)
            .Include(f => f.OrganizationsOfTheYear)
            .ThenInclude(o => o.Year)
            .Select(ConvertDtos.ToFieldOfStudyInfoDto).ToList();

        return fieldOfStudyLogs;
    }

    public List<DayOff> GetDaysOff(int fieldOfStudyLogId)
    {
        var fieldOfStudyLog = _context.FieldOfStudyLogs.FirstOrDefault(f => f.Id == fieldOfStudyLogId);
        if(fieldOfStudyLog == null) throw new HttpNotFoundException($"Field of study log not found: {fieldOfStudyLogId}");
        return _context.DaysOff.Where(d => d.OrganizationsOfTheYearId == fieldOfStudyLog.OrganizationsOfTheYearId)
            .ToList();
    }

    public async Task<List<FieldOfStudyLog>> GetFieldsOfStudyLogs()
    {
        return await _context.FieldOfStudyLogs
            .AsNoTracking()
            .Include(f=>f.FieldOfStudy)
            .Include(f => f.OrganizationsOfTheYear)
            .ThenInclude(o => o.Year)
            .ToListAsync();
    }

    public FieldOfStudy GetFieldOfStudy(int id)
    {
        var field = _context.FieldOfStudies.FirstOrDefault(f => f.Id == id);
        if(field == null) throw new HttpNotFoundException($"Field of study not found: {id}");
        return field;
    }

    public async Task CreateFieldOfStudyLog(FieldOfStudy fieldOfStudy, OrganizationOfTheYear organization, byte semester)
    {
        var fieldOfStudyLog = new FieldOfStudyLog
        {
            FieldOfStudyId = fieldOfStudy.Id,
            OrganizationsOfTheYearId = organization.Id,
            Semester = semester,
        };
        _context.Add(fieldOfStudyLog);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFieldOfStudyLog(int fieldOfStudyLog)
    {
        var fieldOfStudy = GetFieldOfStudyLogById(fieldOfStudyLog);
        _context.Remove(fieldOfStudy);
        await _context.SaveChangesAsync();
    }

    public async void DeleteAllFieldOfStudyLogs()
    {
        _context.FieldOfStudyLogs.RemoveRange(_context.FieldOfStudyLogs);
        _context.SaveChanges();
    }
}