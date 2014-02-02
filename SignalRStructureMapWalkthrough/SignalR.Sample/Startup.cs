using Owin;

namespace Microsoft.AspNet.SignalR.StockTicker
{
    public static class Startup
    {
        public static void ConfigureSignalR(IAppBuilder app, HubConfiguration config)
        {
            //Changed this method to accept a HubConfiguration parameter
            //and pass it off to the MapSignalR method
            app.MapSignalR(config);
        }
    }
}