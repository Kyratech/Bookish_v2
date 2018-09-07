using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bookish_V2.Web.Startup))]
namespace Bookish_V2.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
