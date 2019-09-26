using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProyectoIntegrador.Startup))]
namespace ProyectoIntegrador
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
