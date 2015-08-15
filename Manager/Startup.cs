using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Manager.Startup))]
namespace Manager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
