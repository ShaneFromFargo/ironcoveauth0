﻿using System.Configuration;
using System.Linq.Expressions;
using System.Net;
using Auth0.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(PizzaAuth.Startup))]

namespace PizzaAuth
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Auth0 parameters
            string auth0Domain = ConfigurationManager.AppSettings["auth0:Domain"];
            string auth0ClientId = ConfigurationManager.AppSettings["auth0:ClientId"];
            string auth0ClientSecret = ConfigurationManager.AppSettings["auth0:ClientSecret"];

            // Enable Kentor Cookie Saver middleware
            app.UseKentorOwinCookieSaver();

            // Set Cookies as default authentication type
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                LoginPath = new PathString("/Account/Login")
            });

            // Configure Auth0 authentication
            var options = new Auth0AuthenticationOptions()
            {
                Domain = auth0Domain,
                ClientId = auth0ClientId,
                ClientSecret = auth0ClientSecret,
                

                //SaveIdToken = true,
                //SaveAccessToken = true,
                //SaveRefreshToken = true,
                // If you want to request an access_token to pass to an API, then replace the audience below to 
                // pass your API Identifier instead of the /userinfo endpoint
                Provider = new Auth0AuthenticationProvider()
                {
                    OnApplyRedirect = context =>
                    {
                        string userInfoAudience = $"https://{auth0Domain}/userinfo";
                        string redirectUri = context.RedirectUri + "&audience=" + WebUtility.UrlEncode(userInfoAudience);

                        context.Response.Redirect(redirectUri);
                    }
                }
            };

      
            //This adds the email to the custom claims
            options.Scope.Add("email");

            //For some reason this is not adding the gender to the claims. 
            options.Scope.Add("gender");
            app.UseAuth0Authentication(options);
        }
    }
}
