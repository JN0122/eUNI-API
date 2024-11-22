using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Student;

namespace eUNI_API.Services.Interfaces;

public interface IStudentService
{
    public Task<StudentInfoDto> GetStudentInfo(Guid userId);
    // groups, student fields of study
}