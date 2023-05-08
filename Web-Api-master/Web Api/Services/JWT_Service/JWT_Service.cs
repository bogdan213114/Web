using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Web_Api.Models.AuthenticationModels;
using Web_Api.Models.Validation;

namespace Web_Api.Services;

public class JWT_Service : IJWT_Service
{
    private readonly string _JWT_secred;
    public readonly int JWT_LifeTimeMinutes;


    public JWT_Service(IConfiguration config)
    {
        _JWT_secred = config["JWT:Secred"];
        if (_JWT_secred.Length % 4 != 0)
        {
            throw new ConfigurationErrorsException("JWT secred length must be multiple of 4.");
        }
        JWT_LifeTimeMinutes = Int32.Parse(config["JWT:LifetimeMinutes"]);
        if (JWT_LifeTimeMinutes <= 0)
        {
            throw new ConfigurationErrorsException("LifetimeMinutes must be biggen than 0.");
        }
    }
    
    public JsonWebToken GenerateJWT(Account account)
    {
        return GenerateJWT(account, DateTime.Now);
    }
    public JsonWebToken GenerateJWT(Account account, DateTime now)
    {
        DateTimeOffset iat = new(now);
        DateTimeOffset exp = iat.AddMinutes(JWT_LifeTimeMinutes);

        string jwt_Value = JwtBuilder.Create()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret(_JWT_secred)
            .AddClaim("id", account.Id)
            .AddClaim("name", account.Name)
            .AddClaim("iat", iat.ToUnixTimeSeconds())
            .AddClaim("exp", exp.ToUnixTimeSeconds())
            .Encode();
        return new JsonWebToken(jwt_Value, account.Id, account.Name, iat, exp);
    }
    public async Task<JsonWebToken> GenerateJWT_Async(Account account)
    {
        return await Task.Run(() =>
        {
            return GenerateJWT(account);
        });
    }
    public async Task<JsonWebToken> GenerateJWT_Async(Account account, DateTime now)
    {
        return await Task.Run(() =>
        {
            return GenerateJWT(account, now);
        });
    }
}
