using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using eUNI_API.Configuration;
using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace eUNI_API.Services;

public class TokenService(AppDbContext context, IOptions<JwtSettings> jwtSettings, IStudentService studentService, IRepresentativeService representativeService): ITokenService
{
    private readonly AppDbContext _context = context;
    private readonly IStudentService _studentService = studentService;
    private readonly IRepresentativeService _representativeService = representativeService;
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public string CreateAccessToken(Guid userId)
    {
        var user = _context.Users.AsNoTracking().Include(u => u.Role).FirstOrDefault(u => u.Id == userId); 
        
        if (user == null)
            throw new ArgumentException("User not found");
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        var isRepresentative = _representativeService.IsRepresentative(user.Id);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("IsRepresentative", isRepresentative.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience 
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public string CreateRefreshToken(Guid userId)
    {
        var token = GenerateUniqueRefreshToken();

        _context.RefreshTokens.Add(new RefreshToken
        {
            Token = token,
            UserId = userId,
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        });
        _context.SaveChanges();
        
        return token;
    }

    public string RegenerateRefreshToken(string refreshToken)
    {
        var refreshTokenEntity = _context.RefreshTokens
            .First(r => r.Token == refreshToken);
        
        var newToken = GenerateUniqueRefreshToken();
        refreshTokenEntity.Token = newToken;
        refreshTokenEntity.Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
        
        _context.RefreshTokens.Update(refreshTokenEntity);
        _context.SaveChanges();
        
        return newToken;
    }

    public void RevokeUserTokens(Guid userId)
    {
        var tokens = _context.RefreshTokens.Where(r => r.UserId == userId).ToList();
        
        if (tokens.Count == 0)
            return;
        
        _context.RefreshTokens.RemoveRange(tokens);
        _context.SaveChanges();
    }

    public Guid GetUserIdFromRefreshToken(string refreshToken)
    {
        RefreshToken? refreshTokenEntity = _context.RefreshTokens
            .FirstOrDefault(r => r.Token == refreshToken);
        
        if(refreshTokenEntity == null)
            throw new ArgumentException("Refresh token doesn't exist");
        
        return refreshTokenEntity.UserId;
    }

    public void RevokeRefreshToken(string? refreshToken)
    {
        if (refreshToken == null)
            return;
        
        RefreshToken? refreshTokenEntity = _context.RefreshTokens
            .FirstOrDefault(r => r.Token == refreshToken);

        if (refreshTokenEntity == null)
            return;
        
        _context.RefreshTokens.Remove(refreshTokenEntity);
        _context.SaveChanges();
    }

    public bool IsRefreshTokenValid(string refreshToken)
    {
        var refreshTokenEntity = _context.RefreshTokens.AsNoTracking().FirstOrDefault(t => t.Token == refreshToken);
        return refreshTokenEntity != null && refreshTokenEntity.Expires > DateTime.UtcNow;
    }

    public string GenerateUniqueRefreshToken()
    {
        string token;
        while(true)
        {
            token = TokenGenerator.GenerateRefreshToken();
            var rTokenEntity = _context.RefreshTokens
                .AsNoTracking()
                .FirstOrDefault(rtoken => rtoken.Token == token);

            if (rTokenEntity == null)
                break;
        }
        return token;
    }
}
