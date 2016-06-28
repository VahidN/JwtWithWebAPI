using System.Security.Claims;
using System.Web.Http;
using JwtWithWebAPI.JsonWebTokenConfig;
using JwtWithWebAPI.Services;

namespace JwtWithWebAPI.Controllers
{
    [JwtAuthorize(Roles = "Admin")]
    public class MyProtectedAdminApiController : ApiController
    {
        private readonly IUsersService _usersService;

        public MyProtectedAdminApiController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public IHttpActionResult Get()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userDataClaim = claimsIdentity.FindFirst(ClaimTypes.UserData);
            var userId = userDataClaim.Value;

            return Ok(new
            {
                Id = 1,
                Title = "Hello from My Protected Admin Api Controller!",
                Username = this.User.Identity.Name,
                UserData = userId,
                TokenSerialNumber = _usersService.GetSerialNumber(int.Parse(userId))
            });
        }
    }
}