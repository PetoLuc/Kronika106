using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kronika106.Startup))]
namespace Kronika106
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
