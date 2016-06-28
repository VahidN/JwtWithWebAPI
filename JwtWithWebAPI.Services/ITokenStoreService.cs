using JwtWithWebAPI.DomainClasses;

namespace JwtWithWebAPI.Services
{
    public interface ITokenStoreService
    {
        void CreateUserToken(UserToken userToken);
        bool IsValidToken(string accessToken, int userId);
        void DeleteExpiredTokens();
        UserToken FindToken(string refreshTokenIdHash);
        void DeleteToken(string refreshTokenIdHash);
        void InvalidateUserTokens(int userId);
        void UpdateUserToken(int userId, string accessTokenHash);
    }
}