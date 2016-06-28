using JwtWithWebAPI;
using JwtWithWebAPI.IoCConfig;
using JwtWithWebAPI.JsonWebTokenConfig;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OwinStartup))]
namespace JwtWithWebAPI
{
    public class OwinStartup
    {
        /// <summary>
        /// PM> Install-Package Microsoft.Owin.Host.SystemWeb
        /// PM> Install-Package Microsoft.Owin.Security.Jwt
        /// </summary>
        public void Configuration(IAppBuilder app)
        {
            app.UseOAuthAuthorizationServer(SmObjectFactory.Container.GetInstance<AppOAuthOptions>());
            app.UseJwtBearerAuthentication(SmObjectFactory.Container.GetInstance<AppJwtOptions>());
        }
    }
}