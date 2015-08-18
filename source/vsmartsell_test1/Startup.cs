using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(vsmartsell_test1.Startup))]
namespace vsmartsell_test1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
