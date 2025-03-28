using eUNI_API.Models.Entities.Auth;

namespace eUNI_API.Repositories.Interfaces;

public interface IAuthRepository
{
    public Task<User> GetUserWithRole(string email);
    public bool IsAdmin(Guid userId);
    public void RevokeAllTokens();
}