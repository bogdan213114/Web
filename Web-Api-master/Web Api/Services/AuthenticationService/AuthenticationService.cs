using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Web_Api.Models;
using Web_Api.Models.AuthenticationModels;

namespace Web_Api.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly AccountRepository _accountRepository;
    private readonly UserRepository _userRepository;

    private readonly HashService _hashService;
    private readonly JWT_Service _JWTService;
    private readonly RT_Service _RT_Service;
    private readonly ValidationService _validationService;

    public AuthenticationService(
        AccountRepository accountRepository,
        UserRepository userRepository,
        HashService hashService,
        JWT_Service JWT_Service,
        RT_Service RT_Service,
        ValidationService validationService)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _hashService = hashService;
        _JWTService = JWT_Service;
        _RT_Service = RT_Service;
        _validationService = validationService;
    }
    public async Task<IActionResult> CheckUsernameOccupationAsync(string username)
    {
        if (!_validationService.ValidateUsername(username).IsValid)
        {
            return new OkObjectResult(new OccupationCheckResponse { IsFree = false, IsValid = false });
        }
        if (await _accountRepository.IsUsernameFreeAsync(username))
        {
            return new OkObjectResult(new OccupationCheckResponse { IsFree = true, IsValid = true });
        }
        return new OkObjectResult(new OccupationCheckResponse { IsFree = false, IsValid = true });
    }
    public async Task<IActionResult> RegisterAsync(string userName, string passwd)
    {
        switch (_validationService.ValidateRegistrationData(userName, passwd))
        {
            case { IsNameValid: false, IsPasswdValid: false }:
                return new BadRequestObjectResult(RegistrationResponse.PasswdAndLoginNotValidated());
            case { IsNameValid: false }:
                return new BadRequestObjectResult(RegistrationResponse.LoginNotValidated());
            case { IsPasswdValid: false }:
                return new BadRequestObjectResult(RegistrationResponse.PasswdNotValidated());
        }
        if (!await _accountRepository.IsUsernameFreeAsync(userName))
        {
            return new BadRequestObjectResult(RegistrationResponse.UsernameTaken());
        }
        Account account = new()
        {
            Name = userName,
            PasswdHash = await _hashService.GetPasswdHashAsync(passwd),
            User = new User { Name = userName, RegisteredAt = DateTime.Now }
        };
        await _accountRepository.CreateAsync(account);
        await _userRepository.SaveAsync();


        string rt_Value;
        do
        {
            rt_Value = _RT_Service.GenerateRT_Value();
        }
        while (!await _accountRepository.IsRT_UniqueAsync(rt_Value));
        
        var now = DateTime.Now;
        var rt = _RT_Service.CreateToken(rt_Value, now);
        account.RefreshTokens = new List<RefreshToken>(1){rt};
        await _accountRepository.SaveAsync();
        var jwt = await _JWTService.GenerateJWT_Async(account, now);
        return new OkObjectResult(new RegistrationResponse(jwt, rt));
    }
    public async Task<IActionResult> AuthentificateAsync(string userName, string passwd)
    {
        switch (_validationService.ValidateAuthenticationData(userName, passwd))
        {
            case { IsValid: true }:
                break;
            case { IsNameValid: false }:
                return new BadRequestObjectResult(AuthenticationResponse.WrongName());
            case { IsPasswdValid: false }:
                return new BadRequestObjectResult(AuthenticationResponse.WrongPasswd());
        }
        Account account = await _accountRepository.GetAccountByUserNameAsync(userName);

        if (account is null)
        {
            return new BadRequestObjectResult(AuthenticationResponse.WrongName());
        }
        if (_hashService.GetPasswdHash(passwd) != account.PasswdHash)
        {
            return new BadRequestObjectResult(AuthenticationResponse.WrongPasswd());
        }

        RefreshToken oldRT = account.RefreshTokens.Single();

        string rt_Value;
        do
        {
            rt_Value = _RT_Service.GenerateRT_Value();
        }
        while (!await _accountRepository.IsRT_UniqueAsync(rt_Value));

        var now = DateTime.Now;
        RefreshToken newRT = _RT_Service.CreateToken(rt_Value, now);
        account.RefreshTokens.Add(newRT);
        oldRT.ReplaceBy(newRT);
        await _accountRepository.SaveAsync();

        var jwt = await _JWTService.GenerateJWT_Async(account, now);
        return new OkObjectResult(new AuthenticationResponse(jwt, newRT));
    }
    public async Task<IActionResult> RefreshAsync(string refreshTokenValue)
    {
        if (!_validationService.ValidateRT_Value(refreshTokenValue).IsValid)
        {
            return new BadRequestObjectResult(RefreshResponse.NotValidated());
        }

        RefreshToken oldRT = await _accountRepository.GetRefreshToken(refreshTokenValue);
        if (oldRT is null)
        {
            return new BadRequestObjectResult(RefreshResponse.NotFound());
        }

        if (oldRT.IsExpired())
        {
            return new BadRequestObjectResult(RefreshResponse.Expired());
        }

        string rt_Value;
        do
        {
            rt_Value = _RT_Service.GenerateRT_Value();
        }
        while (!await _accountRepository.IsRT_UniqueAsync(rt_Value));

        Account account = oldRT.Account;
        var now = DateTime.Now;
        RefreshToken newRT = _RT_Service.CreateToken(rt_Value, now);
        account.RefreshTokens.Add(newRT);
        oldRT.ReplaceBy(newRT);
        await _accountRepository.SaveAsync();
        var jwt = await _JWTService.GenerateJWT_Async(account, now);
        return new OkObjectResult(new RefreshResponse(jwt, newRT));
    }
}
