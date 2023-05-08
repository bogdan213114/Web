namespace Web_Api.Models.Validation
{
    public struct RT_ValueValidationResult : IValidationResult
    {
        public bool IsValid { get; set; }
        public bool IsBase64String { get; set; }
        public bool IsLengthValidated { get; set; }
    }
}
