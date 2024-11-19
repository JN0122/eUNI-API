using System.Security.Claims;
using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class RepresentativeService(AppDbContext context, IAdminService adminService, IOrganizationService organizationService, IStudentService studentService): IRepresentativeService
{
    private readonly AppDbContext _context = context;
    private readonly IAdminService _adminService = adminService;
    private readonly IOrganizationService _organizationService = organizationService;
    private readonly IStudentService _studentService = studentService;

    public async Task<List<FieldOfStudyInfoDto>?> GetFieldOfStudyLogToEdit(Guid userId)
    {
        var newestAcademicOrganizationId = _organizationService.GetNewestOrganizationId();
       
        var fieldOfStudyLogs = await _context.FieldOfStudyLogs
            .AsNoTracking()
            .Where(f => f.OrganizationsOfTheYearId == newestAcademicOrganizationId)
            .Include(f => f.FieldOfStudy)
            .Select(f => ConvertDtos.ToFieldOfStudyInfoDto(f))
            .ToListAsync();

        if (_adminService.IsAdmin(userId)) return fieldOfStudyLogs;
        return await _studentService.GetStudentFieldsOfStudy(userId);
    }

    public async Task<bool> IsRepresentative(Guid userId)
    {
        var fieldsOfStudy = await GetFieldOfStudyLogToEdit(userId);
        return fieldsOfStudy != null;
    }
}