using eUNI_API.Models.Entities.User;

namespace eUNI_API.Services.Interfaces;

public interface ITokenService
{
    public string CreateAccessToken(User user);
    public string CreateRefreshToken(User user);
    public User GetUserByRefreshToken(string refreshToken);
    public void DeleteRefreshToken(string oldRefreshToken);
    public bool IsRefreshTokenValid(string refreshToken);
    public string GenerateUniqueRefreshToken();
}