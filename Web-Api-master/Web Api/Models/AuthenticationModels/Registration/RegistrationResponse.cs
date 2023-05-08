namespace Web_Api.Models.AuthenticationModels;

public struct RegistrationResponse : IResponse
{
    public JsonWebToken? JWT { get; set; }
    public RefreshToken RT { get; set; }
    public State State { get; set; }

    public static RegistrationResponse PasswdNotValidated()
    {
        return new RegistrationResponse
        {
            State = State.RegistrationFailedPasswdNotValidated,
            JWT = null,
            RT = null
        };
    }
    public static RegistrationResponse LoginNotValidated()
    {
        return new RegistrationResponse()
        {
            State = State.RegistrationFailedLoginNotValidated,
            JWT = null,
            RT=null
        };
    }
    public static RegistrationResponse PasswdAndLoginNotValidated()
    {
        return new RegistrationResponse()
        {
            State = State.RegistrationFailedLoginAndPasswdNotValidated,
            JWT = null,
            RT=null
        };
    }
    public static RegistrationResponse UsernameTaken()
    {
        return new RegistrationResponse()
        {
            State = State.RegistrationFailedUsernameTaken,
            JWT = null,
            RT=null
        };
    }

    public RegistrationResponse(JsonWebToken jwt, RefreshToken rt)
    {
        JWT = jwt;
        RT = rt;
        State = State.RegistrationSuccesfull;
    }
}
