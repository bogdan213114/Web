using Web_Api.Models.AuthenticationModels;

namespace Web_Api.Services;

public interface IRT_Service
{
    string GenerateRT_Value();
    Task<string> GenerateRT_ValueAsync();
    RefreshToken CreateToken(string tokenValue, DateTime now);
}
