using Web_Api.Models.AuthenticationModels;

namespace Web_Api.Models.Validation;

public struct JWT_ValidationResult : IValidationResult
{
    public bool IsValid { get; set; }
    public bool? IsSignatureValid { get; set; }
    public bool? IsNotExpired { get; set; }
    public JsonWebToken? JWT { get; set; }
}
