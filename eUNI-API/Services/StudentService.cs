using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Student;
using eUNI_API.Models.Entities.Student;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class StudentService(AppDbContext context, IStudentRepository studentRepository, IRepresentativeService representativeService): IStudentService
{
    private readonly AppDbContext _context = context;
    private readonly IStudentRepository _studentRepository = studentRepository;
    private readonly IRepresentativeService _representativeService = representativeService;
    
    public async Task<StudentInfoDto> GetStudentInfo(Guid userId)
    {
        var isRepresentative = await _representativeService.IsRepresentative(userId);
        var studentId = await _studentRepository.GetStudentId(userId);
        var fieldsOfStudy = await _studentRepository.GetStudentFieldsOfStudy(studentId);
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