using System;
using JwtWithWebAPI.IoCConfig;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;

namespace JwtWithWebAPI.JsonWebTokenConfig
{
    public class AppOAuthOptions : OAuthAuthorizationServerOptions
    {
        public AppOAuthOptions(IAppJwtConfiguration configuration)
        {
            this.AllowInsecureHttp = true; // TODO: Buy an SSL certificate!
            this.TokenEndpointPath = new PathString(configuration.TokenPath);
            this.AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(configuration.ExpirationMinutes);
            this.AccessTokenFormat = new AppJwtWriterFormat(this, configuration);
            this.Provider = SmObjectFactory.Container.GetInstance<IOAuthAuthorizationServerProvider>();
            this.RefreshTokenProvider = SmObjectFactory.Container.GetInstance<IAuthenticationTokenProvider>();
        }
    }
}