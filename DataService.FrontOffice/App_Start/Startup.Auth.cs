using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace twg.chk.DataService.FrontOffice
{
    public partial class Startup
    {
        static Startup()
        {
            PublicClientId = "self";

            //UserManagerFactory = () => new UserManager<IdentityUser>(new UserStore<IdentityUser>());

            //OAuthOptions = new OAuthAuthorizationServerOptions
            //{
            //    TokenEndpointPath = new PathString("/Token"),
            //    Provider = new ApplicationOAuthProvider(PublicClientId, UserManagerFactory),
            //    AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
            //    AllowInsecureHttp = true
            //};
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        //public static Func<UserManager<IdentityUser>> UserManagerFactory { get; set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}