using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Group;
using eUNI_API.Models.Dto.Student;

namespace eUNI_API.Services.Interfaces;

public interface IStudentService
{
    public StudentInfoDto GetStudentInfo(Guid userId);
    public Task ChangeStudentGroup(Guid userId, StudentChangeGroupRequestDto studentChangeGroupRequestDto);
    public Task SetCurrentFieldOfStudy(Guid userId, int fieldOfStudyId);
    // groups, student fields of study
}