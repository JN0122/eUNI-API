using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Group;
using eUNI_API.Models.Dto.Student;
using eUNI_API.Models.Entities.Student;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class StudentService(AppDbContext context, IStudentRepository studentRepository, IOrganizationRepository organizationRepository, IGroupRepository groupRepository): IStudentService
{
    private readonly AppDbContext _context = context;
    private readonly IStudentRepository _studentRepository = studentRepository;
    private readonly IOrganizationRepository _organizationRepository = organizationRepository;
    private readonly IGroupRepository _groupRepository = groupRepository;
    
    public async Task<StudentInfoDto> GetStudentInfo(Guid userId)
    {
        var academicOrganizationId = _organizationRepository.GetNewestOrganizationId();
        var studentId = await _studentRepository.GetStudentId(userId);
        if (studentId == null) throw new ArgumentException("Invalid user");
        var fieldsOfStudy = await _studentRepository.GetStudentFieldsOfStudy(studentId.Value, academicOrganizationId);
        var studentAlbumNumber = _studentRepository.GetAlbumNumber(studentId.Value);

        return new StudentInfoDto
        {
            Id = studentId.Value,
            AlbumNumber = studentAlbumNumber,
            FieldsOfStudyInfo = fieldsOfStudy
        };
    }
    
    public async Task<IEnumerable<GroupDto>> GetGroups(int fieldOfStudyLogId)
    {
        var classes = await _context.Classes
            .AsNoTracking()
            .Where(c => c.FieldOfStudyLogId == fieldOfStudyLogId)
            .ToListAsync();
        return classes.Select(c => _groupRepository.GetGroup(c.Id));
    }
}