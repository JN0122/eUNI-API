using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Group;
using eUNI_API.Models.Dto.Student;

namespace eUNI_API.Services.Interfaces;

public interface IStudentService
{
    public Task<StudentInfoDto> GetStudentInfo(Guid userId);
    public Task ChangeStudentGroup(Guid userId, StudentChangeGroupRequestDto studentChangeGroupRequestDto);
    // groups, student fields of study
}