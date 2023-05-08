using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Services;

public interface IAuthenticationService
{
    Task<IActionResult> CheckUsernameOccupationAsync(string username);
    Task<IActionResult> RegisterAsync(string userName, string passwd);
    Task<IActionResult> AuthentificateAsync(string userName, string passwd);
}
