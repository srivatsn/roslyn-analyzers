using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(RoslynAnalyzersStatus.Web.Startup))]
namespace RoslynAnalyzersStatus.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
