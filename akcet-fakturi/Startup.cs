using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(akcet_fakturi.Startup))]
namespace akcet_fakturi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
