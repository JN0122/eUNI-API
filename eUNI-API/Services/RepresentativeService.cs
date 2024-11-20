using System.Security.Claims;
using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.Classes;
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

    public Task<List<ClassDto>> GetClasses(int fieldOfStudyId)
    {
        throw new NotImplementedException();
    }

    public Task CreateClass(CreateClassRequestDto classRequestDto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateClass(int id, CreateClassRequestDto classRequestDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteClass(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<AssignmentDto>> GetAssignments(int fieldOfStudyLogId)
    {
        throw new NotImplementedException();
    }

    public Task CreateAssignment(CreateAssignmentRequestDto assignmentRequestDto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAssignment(int id, CreateAssignmentRequestDto assignmentRequestDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAssignment(int id)
    {
        throw new NotImplementedException();
    }
}