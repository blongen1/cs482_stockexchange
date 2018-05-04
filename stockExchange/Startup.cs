using System;
using Hangfire;
using Microsoft.Owin;
using Owin;
using stockExchange.Controllers;

[assembly: OwinStartupAttribute(typeof(stockExchange.Startup))]
namespace stockExchange
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
            RecurringJob.AddOrUpdate<StocksController>(x => x.UpdateStockPrices(), Cron.Minutely);
            RecurringJob.AddOrUpdate<StocksController>(x => x.BuildPriceHistory(), Cron.MinuteInterval(15));
            RecurringJob.AddOrUpdate<StocksController>(x => x.CheckPriceAlert(), Cron.Daily(14,30));
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}
