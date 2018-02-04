using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(stockExchange.Startup))]
namespace stockExchange
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
