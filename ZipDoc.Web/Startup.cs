using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ZipDoc.Web.Startup))]
namespace ZipDoc.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
