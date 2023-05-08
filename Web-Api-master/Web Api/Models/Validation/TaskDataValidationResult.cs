namespace Web_Api.Models.Validation;

public struct TaskDataValidationResult : IValidationResult
{
    public bool IsValid { get; set; }
    public bool IsTitleValid { get; set; }
    public bool IsDescriptionValid { get; set; }
}
