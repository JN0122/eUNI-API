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

public class StudentService(IStudentRepository studentRepository): IStudentService
{
    private readonly IStudentRepository _studentRepository = studentRepository;
    
    public StudentInfoDto GetStudentInfo(Guid userId)
    {
        if (!_studentRepository.IsStudent(userId)) throw new ArgumentException("Invalid user");
        var fieldsOfStudy = _studentRepository.GetStudentCurrentFieldsOfStudy(userId);
        
        return new StudentInfoDto
        {
            Id = userId,
            CurrentFieldOfStudyInfo = fieldsOfStudy
        };
    }

    public async Task ChangeStudentGroup(Guid userId, StudentChangeGroupRequestDto studentChangeGroupRequestDto)
    {
        if(!_studentRepository.IsStudent(userId)) throw new ArgumentException("Invalid user");
        var studentFieldOfStudyLog = _studentRepository.GetStudentFieldOfStudyLog(studentChangeGroupRequestDto.FieldOfStudyLogId, userId);
        var studentGroup =
            _studentRepository.GetStudentGroup(studentFieldOfStudyLog.Id, studentChangeGroupRequestDto.GroupType);

        if (studentGroup == null)
            _studentRepository.JoinGroup(studentFieldOfStudyLog.Id, studentChangeGroupRequestDto.GroupId); 
        else
            _studentRepository.ChangeGroup(studentGroup.Id, studentChangeGroupRequestDto.GroupId);
    }

    public async Task SetCurrentFieldOfStudy(Guid userId, int fieldOfStudyId)
    {
        await _studentRepository.SetCurrentFieldOfStudy(userId, fieldOfStudyId);
    }
}