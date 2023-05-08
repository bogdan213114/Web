using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using Web_Api.Models.AuthenticationModels;
using Web_Api.Models.Validation;

namespace Web_Api.Services;

public class ValidationService : IValidationService
{
    private readonly int _RT_valueStringLength;
    private readonly string _JWT_secred;


    public ValidationService(RT_Service RT_Service, IConfiguration config)
    {
        _RT_valueStringLength = RT_Service.ValueStringLength;
        _JWT_secred = config["JWT:Secred"];
    }


    public UsernameValidationResult ValidateUsername(string userName)
    {
        bool nameValidated =
            userName is not null &&
            //Name is 3-25 characters length and contains only latin
            //letters, numbers, whitespaces and _$! characters.
            Regex.IsMatch(userName, @"[A-Za-z0-9_$!]{3,25}");
        return new UsernameValidationResult
        {
            IsValid = nameValidated
        };
    }
    public AuthenticationDataValidationResult ValidateAuthenticationData(string userName, string passwd)
    {
        bool nameValidated = ValidateUsername(userName).IsValid;

        bool passwordValidated =
            passwd is not null &&
            //Password is 6-16 characters length and contains
            //only latin letters, numbers and _$! characters.
            Regex.IsMatch(passwd, @"[A-Za-z0-9_$!]{6,16}")
            //Password contains at least one capitalized letter.
            && Regex.IsMatch(passwd, @".*[A-Z].*")
            //Password contains at least one number.
            && Regex.IsMatch(passwd, @".*[0-9].*");

        return new AuthenticationDataValidationResult
        {
            IsValid = nameValidated && passwordValidated,
            IsNameValid = nameValidated,
            IsPasswdValid = passwordValidated
        };
    }
    public RegistrationDataValidationResult ValidateRegistrationData(string userName, string passwd)
    {
        var result = ValidateAuthenticationData(userName, passwd);
        return new RegistrationDataValidationResult
        {
            IsValid = result.IsValid,
            IsNameValid = result.IsNameValid,
            IsPasswdValid = result.IsPasswdValid
        };
    }
    public TaskDataValidationResult ValidateTaskData(string title, string description)
    {
        bool titleValidated = ValidateGroupData(title).IsValid;
        bool descriptionValidated =
            description is not null &&
            //Description contatins only unicode letters,numbers,
            //line breaks and !?.,:; characters.
            Regex.IsMatch(description, @"^[\p{L}\p{Nd}!?.,:;\-\+\n\r\s]*$");
        return new TaskDataValidationResult
        {
            IsValid = titleValidated && descriptionValidated,
            IsTitleValid = titleValidated,
            IsDescriptionValid = descriptionValidated
        };
    }
    public GroupDataValidationResult ValidateGroupData(string title)
    {
        bool titleValidated =
            title is not null &&
            //Title contatins only unicode letters,numbers and !?.,:; characters.
            Regex.IsMatch(title, @"^[\p{L}\p{Nd}!?.,:;\-\+\s]{2,}$");
        return new GroupDataValidationResult { IsValid = titleValidated };
    }
    public JWT_ValidationResult ValidateJWT(string jwt)
    {
        Dictionary<string, object> JWTDict = new();
        JWT_ValidationResult result = new();
        bool problemFound = false;
        if (String.IsNullOrWhiteSpace(jwt))
        {
            result = new JWT_ValidationResult { IsValid = false };
            problemFound = true;
        }
        else try
            {
                JWTDict = JwtBuilder.Create()
                     .WithAlgorithm(new HMACSHA256Algorithm())
                     .WithSecret(_JWT_secred)
                     .MustVerifySignature()
                     .Decode<Dictionary<string, object>>(jwt);
            }
            catch (JWT.Exceptions.TokenExpiredException)
            {
                result = new JWT_ValidationResult
                {
                    IsValid = false,
                    IsNotExpired = false
                };
                problemFound = true;
            }
            catch (JWT.Exceptions.SignatureVerificationException)
            {
                result = new JWT_ValidationResult
                {
                    IsValid = false,
                    IsSignatureValid = false
                };
                problemFound = true;
            }
            catch (Exception)
            {
                result = new JWT_ValidationResult
                {
                    IsValid = false
                };
                problemFound = true;
            }
        if (!problemFound)
        {
            result = new JWT_ValidationResult
            {
                IsValid = true,
                IsSignatureValid = true,
                IsNotExpired = true,
                JWT = new JsonWebToken
                {
                    AccountId = (long)JWTDict["id"],
                    AccountName = (string)JWTDict["name"],
                    IssuedAt = DateTimeOffset.FromUnixTimeSeconds((long)JWTDict["iat"]),
                    ExpiresAt = DateTimeOffset.FromUnixTimeSeconds((long)JWTDict["exp"]),
                    Value = jwt
                }
            };
        }
        return result;
    }
    public async Task<JWT_ValidationResult> ValidateJWTAsync(string jwt)
    {
        return await Task.Run(() =>
        {
            return ValidateJWT(jwt);
        });
    }
    public RT_ValueValidationResult ValidateRT_Value(string refreshTokenValue)
    {
        bool isBase64String =
            !String.IsNullOrEmpty(refreshTokenValue)
            &&
            Regex.IsMatch(refreshTokenValue, @"^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$");
        bool isLengthValidated = (refreshTokenValue ?? "").Length == _RT_valueStringLength;
        return new RT_ValueValidationResult
        {
            IsValid = isBase64String && isLengthValidated,
            IsBase64String = isBase64String,
            IsLengthValidated = isBase64String
        };
    }
}
