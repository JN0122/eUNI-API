using eUNI_API.Models.Dto.FieldOfStudy;

namespace eUNI_API.Services.Interfaces;

public interface IStudentService
{
    public Task<int> GetStudentId(Guid userId);
    public Task<List<int>?> GetStudentGroupIds(int fieldOfStudyId, Guid userId);
    
    public Task<List<FieldOfStudyInfoDto>?> GetStudentFieldsOfStudy(Guid userId);
    // groups, student fields of study
}