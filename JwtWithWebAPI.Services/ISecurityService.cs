namespace JwtWithWebAPI.Services
{
    public interface ISecurityService
    {
        string GetSha256Hash(string input);
    }
}