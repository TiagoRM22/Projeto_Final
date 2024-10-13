using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Projeto_Final.Startup))]
namespace Projeto_Final
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
