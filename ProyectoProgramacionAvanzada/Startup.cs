using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProyectoProgramacionAvanzada.Startup))]
namespace ProyectoProgramacionAvanzada
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
