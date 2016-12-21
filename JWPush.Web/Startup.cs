using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JWPush.Web.Startup))]
namespace JWPush.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
