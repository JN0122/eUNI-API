namespace eUNI_API.Services.Interfaces;

public interface IRepresentativeService
{
    public Task<List<int>?> GetFieldOfStudyLogIdsToEdit(Guid userId);
}