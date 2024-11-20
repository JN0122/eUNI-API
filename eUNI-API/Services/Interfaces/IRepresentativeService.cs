using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Dto.FieldOfStudy;

namespace eUNI_API.Services.Interfaces;

public interface IRepresentativeService
{
    public Task<List<FieldOfStudyInfoDto>?> GetFieldOfStudyLogToEdit(Guid userId);
    public Task<bool> IsRepresentative(Guid userId);
    public Task<List<ClassDto>> GetClasses(int fieldOfStudyId);
    public Task CreateClass(CreateClassRequestDto classRequestDto);
    public Task UpdateClass(int id, CreateClassRequestDto classRequestDto);
    public Task DeleteClass(int id);
}