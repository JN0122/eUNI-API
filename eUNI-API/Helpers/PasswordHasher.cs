using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace eUNI_API.Helpers;

public static class PasswordHasher
{
    public static string GenerateSalt()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8));
    }
    public static string HashPassword(string password, string salt)
    {
        var hashedPassword = KeyDerivation.Pbkdf2(
            password: password!,
            salt: Convert.FromBase64String(salt),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100_000,
            numBytesRequested: 256 / 8);
        return Convert.ToBase64String(hashedPassword);
    }
    public static bool VerifyHashedPassword(string password, string salt, string usersHashedPassword)
    {
        var hashedPassword= HashPassword(password, salt);
        return hashedPassword.Equals(usersHashedPassword);
    }
}