using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Group;
using eUNI_API.Models.Dto.Student;

namespace eUNI_API.Services.Interfaces;

public interface IStudentService
{
    public Task<StudentInfoDto> GetStudentInfo(Guid userId);

    public Task<IEnumerable<GroupDto>> GetGroups(int fieldOfStudyLogId);
    // groups, student fields of study
}