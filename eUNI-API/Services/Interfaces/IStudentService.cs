using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.Student;

namespace eUNI_API.Services.Interfaces;

public interface IStudentService
{
    public Task<List<int>?> GetStudentGroupIds(int fieldOfStudyId, Guid userId);
    
    public Task<List<FieldOfStudyInfoDto>?> GetStudentFieldsOfStudy(Guid userId);

    public List<int>? GetRepresentativeFieldOfStudyLogId(Guid userId);

    public bool IsRepresentative(Guid userId);
}