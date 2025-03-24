namespace eUNI_API.Services.Interfaces;

public interface ISetupService
{
    public Task ResetRootAccount(string newPassword);
}