namespace Web_Api.Models.AuthenticationModels
{
    public struct RefreshResponse : IResponse
    {
        public State State { get; set; }
        public JsonWebToken? JWT { get; set; }
        public RefreshToken RT { get; set; }

        public static RefreshResponse NotValidated()
        {
            return new RefreshResponse
            {
                State = State.RefreshFailedRT_NotValid,
                JWT = null,
                RT = null
            };
        }
        public static RefreshResponse NotFound()
        {
            return new RefreshResponse
            {
                State = State.RefreshFailedRT_NotFound,
                JWT = null,
                RT = null
            };
        }
        public static RefreshResponse Expired()
        {
            return new RefreshResponse
            {
                State = State.RefreshFailedRT_Expired,
                JWT = null,
                RT = null
            };
        }
        public RefreshResponse(JsonWebToken jwt, RefreshToken rt)
        {
            State = State.RefreshSuccesfull;
            JWT = jwt;
            RT = rt;
        }
    }
}
