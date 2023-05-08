using Web_Api.Models.AuthenticationModels;
using Web_Api.Models.Validation;

namespace Web_Api.Services;

interface IJWT_Service
{
    JsonWebToken GenerateJWT(Account account);
    JsonWebToken GenerateJWT(Account account, DateTime now);
    Task<JsonWebToken> GenerateJWT_Async(Account account);
    Task<JsonWebToken> GenerateJWT_Async(Account account, DateTime now);
}
