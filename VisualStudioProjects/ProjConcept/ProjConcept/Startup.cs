using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjConcept.Startup))]
namespace ProjConcept
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
