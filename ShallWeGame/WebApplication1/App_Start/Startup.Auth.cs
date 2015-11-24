using Microsoft.Owin;
using Owin;
using System;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebApplication1.Providers;


namespace WebApplication1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }


        //Get token path /token
        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions oAuthAuthorizationServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(999),
                Provider = new SimpleAuthorizationServerProvider()
                
            };

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                AuthenticationMode = AuthenticationMode.Active
            });

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthAuthorizationServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());



        }
}
    }