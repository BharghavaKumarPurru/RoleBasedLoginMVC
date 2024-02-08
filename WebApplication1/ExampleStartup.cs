using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(WebApplication1.ExampleStartup))]

namespace WebApplication1
{
    public class ExampleStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure middleware components here

            // For example, you can add authentication middleware
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new Microsoft.Owin.PathString("/Auth/Login"),
            });
        }
    }
}
