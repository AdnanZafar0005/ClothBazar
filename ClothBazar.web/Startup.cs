using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClothBazar.web.Startup))]
namespace ClothBazar.web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
