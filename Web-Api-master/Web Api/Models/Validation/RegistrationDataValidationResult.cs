namespace Web_Api.Models.Validation;

public struct RegistrationDataValidationResult : IValidationResult
{
    public bool IsValid { get; set; }
    public bool IsNameValid { get; set; }
    public bool IsPasswdValid { get; set; }
}
