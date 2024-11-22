using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Repositories;

public class FieldOfStudyRepository(AppDbContext context): IFieldOfStudyRepository
{
    private readonly AppDbContext _context = context;
    
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
}