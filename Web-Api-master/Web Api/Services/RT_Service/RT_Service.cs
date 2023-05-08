using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Security.Cryptography;
using Web_Api.Models.AuthenticationModels;

namespace Web_Api.Services;

public class RT_Service : IRT_Service
{
    private readonly int _valueStringLength;
    private readonly int _bytesLength;
    private readonly RandomNumberGenerator _rnd;
    private readonly long _RT_LifeTimeMinutes;


    public int ValueStringLength
    {
        get { return _valueStringLength; }
    }


    public RT_Service(IConfiguration config, RandomNumberGenerator rnd)
    {
        _valueStringLength = Int32.Parse(config["RefreshToken:StringLength"]);
        if (_valueStringLength <= 0 || _valueStringLength % 4 != 0)
        {
            throw new ConfigurationErrorsException("StringLength must be bigger than 0 and multiple of 4.");
        }

        _bytesLength = (int)Math.Ceiling(_valueStringLength * 3m / 4m);

        _RT_LifeTimeMinutes = Int64.Parse(config["RefreshToken:LifetimeMinutes"]);
        if (_RT_LifeTimeMinutes <= 0)
        {
            throw new ConfigurationErrorsException("RT LifetimeMinutes must be biggen than 0.");
        }

        _rnd = rnd;
    }
    public string GenerateRT_Value()
    {
        byte[] temp = new byte[_bytesLength];
        _rnd.GetBytes(temp);
        return Convert.ToBase64String(temp);
    }
    public async Task<string> GenerateRT_ValueAsync()
    {
        return await Task.Run(() => GenerateRT_Value());
    }
    public RefreshToken CreateToken(string tokenValue)
    {
        return new RefreshToken
        {
            Value = tokenValue,
            IssuedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddMinutes(_RT_LifeTimeMinutes)
        };
    }
    public RefreshToken CreateToken(string tokenValue, DateTime now)
    {
        return new RefreshToken
        {
            Value = tokenValue,
            IsActive = true,
            IssuedAt = now,
            ExpiresAt = now.AddMinutes(_RT_LifeTimeMinutes)
        };
    }
    

}
