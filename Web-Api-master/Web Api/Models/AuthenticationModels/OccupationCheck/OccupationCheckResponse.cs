namespace Web_Api.Models.AuthenticationModels;

public struct OccupationCheckResponse : IResponse
{
    public State State { get; set; } = State.CheckedSuccesfully;
    public bool IsValid { get; set; }
    public bool IsFree { get; set; }
}
