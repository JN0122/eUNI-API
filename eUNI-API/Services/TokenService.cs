using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using eUNI_API.Configuration;
using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Entities.User;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace eUNI_API.Services;

public class TokenService(AppDbContext context, IOptions<JwtSettings> jwtSettings): ITokenService
{
    private readonly AppDbContext _context = context;
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public string CreateAccessToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience 
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public string CreateRefreshToken(User user)
    {
        var token = GenerateUniqueRefreshToken();

        _context.RefreshTokens.Add(new RefreshToken
        {
            Token = token,
            User = user,
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        });
        _context.SaveChanges();
        
        return token;
    }

    public string GenerateUniqueRefreshToken()
    {
        string token;
        while(true)
        {
            token = TokenGenerator.GenerateRefreshToken();
            var rTokenEntitie = _context.RefreshTokens
                .AsNoTracking()
                .FirstOrDefault(rtoken => rtoken.Token == token);

            if (rTokenEntitie == null)
                break;
        }
        return token;
    }
}
