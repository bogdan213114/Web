namespace Web_Api.Models.AuthenticationModels
{
    public enum State : int
    {
        RegistrationSuccesfull = 11,
        AuthenticationSuccesfull = 12,
        RefreshSuccesfull = 13,
        CheckedSuccesfully = 14,

        RegistrationFailedLoginNotValidated = 21,
        RegistrationFailedPasswdNotValidated = 22,
        RegistrationFailedLoginAndPasswdNotValidated = 23,
        RegistrationFailedUsernameTaken = 24,
        AuthenticationFailedWrongPasswd = 25,
        AuthenticationFailedWrongName = 26,

        RefreshFailedRT_NotFound = 31,
        RefreshFailedRT_Expired = 32,
        RefreshFailedRT_NotValid = 33
    }
}
