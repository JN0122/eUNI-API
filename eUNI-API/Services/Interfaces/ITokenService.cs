using eUNI_API.Models.Entities.User;

namespace eUNI_API.Services.Interfaces;

public interface ITokenService
{
    public string CreateAccessToken(User user);
    public string CreateRefreshToken();
}