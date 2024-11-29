using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Group;

namespace eUNI_API.Services.Interfaces;

public interface IFieldOfStudyServices
{
    public Task<IEnumerable<GroupDto>> GetGroups(int fieldOfStudyLogId);
    public IEnumerable<FieldOfStudyInfoDto> GetAllFieldsOfStudyLogs();
}