using Microsoft.AspNetCore.Mvc;
using Web_Api.Models.AuthenticationModels;
using Web_Api.Models.DataModels;
using Web_Api.Services;

namespace Web_Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly AuthenticationService _authenticationService;
    public AuthenticationController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    /// <summary>
    /// Checks specified username for occupation.
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    /// <response code="200">Returns <code>IsUsernameFreeResponse</code> object with information about username occupation and validation.</response>
    [HttpGet("checkUsername")]
    public async Task<IActionResult> CheckUsername(string userName)
    {
        return await _authenticationService.CheckUsernameOccupationAsync(userName);
    }

    /// <summary>
    /// Register new user.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Returns <code>RegistrationResponse</code> with successful state.</response>
    /// <response code="400">If name or passwd not validated or username already taken.</response>
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegistrationRequest request)
    {
        return await _authenticationService.RegisterAsync(request.Name, request.Passwd);
    }

    /// <summary>
    /// Authenticate user.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Returns <code>AuthenticationResponse</code> with successful state, JWT and refresh token.</response>
    /// <response code="400">If the given username and passwd not pass.</response>
    [HttpPost("login")]
    public async Task<IActionResult> Authentificate([FromBody] AuthenticationRequest request)
    {
        return await _authenticationService.AuthentificateAsync(request.Name, request.Passwd);
    }

    /// <summary>
    /// Get new refresh and jwt tokens.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Returns <code>RefreshResponse</code> with successful state, JWT and refresh token.</response>
    /// <response code="400">If the given refresh token is expired or not found or broken.</response>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest request)
    {
        return await _authenticationService.RefreshAsync(request.RefreshToken);
    }
}
