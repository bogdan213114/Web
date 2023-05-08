using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace Web_Api.Services;

public class HashService : IHashService
{
    private readonly string _passwdSalt;

    public HashService(IConfiguration configuration)
    {
        _passwdSalt = configuration["PasswdSalt"];
        if (_passwdSalt.Length % 4 != 0)
        {
            throw new ConfigurationErrorsException("Passwd salt length must be multiple of 4.");
        }
    }
    public string GetPasswdHash(string passwd)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: passwd,
            salt: Convert.FromBase64String(_passwdSalt),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 1,
            numBytesRequested: 42));
    }
    public async Task<string> GetPasswdHashAsync(string passwd)
    {
        return await Task.Run(() => GetPasswdHash(passwd));
    }
}
