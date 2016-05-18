using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Evaluate.Startup))]
namespace Evaluate
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
