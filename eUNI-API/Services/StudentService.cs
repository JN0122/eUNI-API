using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Student;
using eUNI_API.Models.Entities.Student;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class StudentService(AppDbContext context, IStudentRepository studentRepository): IStudentService
{
    private readonly AppDbContext _context = context;
    private readonly IStudentRepository _studentRepository = studentRepository;
    
    public async Task<StudentInfoDto> GetStudentInfo(Guid userId)
    {
        var isRepresentative = true; //await _representativeService.IsRepresentative(userId);
        var fieldsOfStudy = await _studentRepository.GetStudentFieldsOfStudy(userId);
        var studentId = await _studentRepository.GetStudentId(userId);
        var studentAlbumNumber = _studentRepository.GetAlbumNumber(studentId);
        var groups = new List<FieldOfStudyGroupDto>();
        
        if(fieldsOfStudy != null)
            foreach(var fieldOfStudy in fieldsOfStudy)
            {
                groups.Add(new FieldOfStudyGroupDto
                {
                    FieldOfStudyLogId = fieldOfStudy.FieldOfStudyLogId,
                    GroupIds = await _studentRepository.GetStudentGroupIds(fieldOfStudy.FieldOfStudyLogId, userId)
                });
                
            }

        return new StudentInfoDto
        {
            Id = studentId,
            AlbumNumber = studentAlbumNumber,
            IsRepresentative = isRepresentative,
            FieldsOfStudyInfo = fieldsOfStudy,
            FieldsOfStudyGroups = groups,
        };
    }
}