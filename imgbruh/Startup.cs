using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(imgbruh.Startup))]
namespace imgbruh
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
