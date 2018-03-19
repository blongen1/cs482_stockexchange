using Hangfire;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(stockExchange.Startup))]
namespace stockExchange
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}
