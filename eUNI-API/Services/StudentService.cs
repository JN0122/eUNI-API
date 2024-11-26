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

public class StudentService(IStudentRepository studentRepository, IOrganizationRepository organizationRepository): IStudentService
{
    private readonly IStudentRepository _studentRepository = studentRepository;
    private readonly IOrganizationRepository _organizationRepository = organizationRepository;
    
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

    public async Task ChangeStudentGroup(Guid userId, StudentChangeGroupRequestDto studentChangeGroupRequestDto)
    {
        var studentId = await _studentRepository.GetStudentId(userId);
        if(studentId == null) throw new ArgumentException("Invalid user");
        var studentFieldOfStudyLog = _studentRepository.GetStudentFieldOfStudyLog(studentChangeGroupRequestDto.FieldOfStudyLogId, studentId.Value);
        var studentGroup =
            _studentRepository.GetStudentGroup(studentFieldOfStudyLog.Id, studentChangeGroupRequestDto.GroupType);

        if (studentGroup == null)
            _studentRepository.JoinGroup(studentFieldOfStudyLog.Id, studentChangeGroupRequestDto.GroupId); 
        else
            _studentRepository.ChangeGroup(studentGroup.Id, studentChangeGroupRequestDto.GroupId);
    }
}