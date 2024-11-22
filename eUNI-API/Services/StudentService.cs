using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Student;
using eUNI_API.Models.Entities.Student;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class StudentService(AppDbContext context, IStudentRepository studentRepository, IOrganizationRepository organizationRepository): IStudentService
{
    private readonly AppDbContext _context = context;
    private readonly IStudentRepository _studentRepository = studentRepository;
    private readonly IOrganizationRepository _organizationRepository = organizationRepository;
    
    public async Task<StudentInfoDto> GetStudentInfo(Guid userId)
    {
        var academicOrganizationId = _organizationRepository.GetNewestOrganizationId();
        var isRepresentative = _studentRepository.IsRepresentative(userId, academicOrganizationId);
        var studentId = await _studentRepository.GetStudentId(userId);
        var fieldsOfStudy = await _studentRepository.GetStudentFieldsOfStudy(studentId, academicOrganizationId);
        var studentAlbumNumber = _studentRepository.GetAlbumNumber(studentId);

        return new StudentInfoDto
        {
            Id = studentId,
            AlbumNumber = studentAlbumNumber,
            IsRepresentative = isRepresentative,
            FieldsOfStudyInfo = fieldsOfStudy
        };
    }
}