namespace Web_Api.Models.AuthenticationModels;

public struct AuthenticationResponse : IResponse
{
    public JsonWebToken? JWT { get; set; }
    public RefreshToken RT { get; set; }
    public State State { get; set; }

    public static AuthenticationResponse WrongPasswd()
    {
        return new AuthenticationResponse
        {
            State = State.AuthenticationFailedWrongPasswd,
            JWT = null
        };
    }
    public static AuthenticationResponse WrongName()
    {
        return new AuthenticationResponse()
        {
            State = State.AuthenticationFailedWrongName,
            JWT = null
        };
    }

    public AuthenticationResponse(JsonWebToken jwt, RefreshToken rt)
    {
        JWT = jwt;
        RT = rt;
        State = State.AuthenticationSuccesfull;
    }
}
