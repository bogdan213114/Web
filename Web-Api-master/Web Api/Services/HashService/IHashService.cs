namespace Web_Api.Services
{
    public interface IHashService
    {
        string GetPasswdHash(string passwd);
        Task<string> GetPasswdHashAsync(string passwd);
    }
}
