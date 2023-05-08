using Web_Api.Models.Validation;

namespace Web_Api.Services;

public interface IValidationService
{
    UsernameValidationResult ValidateUsername(string userName);
    AuthenticationDataValidationResult ValidateAuthenticationData(string userName, string passwd);
    RegistrationDataValidationResult ValidateRegistrationData(string userName, string passwd);
    TaskDataValidationResult ValidateTaskData(string title, string description);
    GroupDataValidationResult ValidateGroupData(string title);
    JWT_ValidationResult ValidateJWT(string jwt);
    Task<JWT_ValidationResult> ValidateJWTAsync(string jwt);
}
