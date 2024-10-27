using eUNI_API.Models.Entities.User;

namespace eUNI_API.Services.Interfaces;

public interface ITokenService
{
    public string CreateAccessToken(Guid userId);
    public string CreateRefreshToken(Guid userId);
    public void RevokeUserTokens(Guid userId);
    public Guid GetUserIdFromRefreshToken(string refreshToken);
    public void RevokeRefreshToken(string? oldRefreshToken);
    public bool IsRefreshTokenValid(string refreshToken);
    public string GenerateUniqueRefreshToken();
}