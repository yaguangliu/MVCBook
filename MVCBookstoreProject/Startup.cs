using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCBookstoreProject.Startup))]
namespace MVCBookstoreProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
