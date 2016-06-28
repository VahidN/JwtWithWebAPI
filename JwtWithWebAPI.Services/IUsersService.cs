using System.Collections.Generic;
using JwtWithWebAPI.DomainClasses;

namespace JwtWithWebAPI.Services
{
    public interface IUsersService
    {
        string GetSerialNumber(int userId);
        IEnumerable<string> GetUserRoles(int userId);
        User FindUser(string username, string password);
        User FindUser(int userId);
        void UpdateUserLastActivityDate(int userId);
    }
}