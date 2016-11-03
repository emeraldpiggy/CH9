using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

namespace CH9.HubService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseErrorPage();

            var hubConfiguration = new HubConfiguration
            {
#if DEBUG
                EnableDetailedErrors = true
#else
                EnableDetailedErrors = false
#endif
            };

            app.MapSignalR(hubConfiguration);
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR(hubConfiguration);

            // Turn tracing on programmatically
            GlobalHost.TraceManager.Switch.Level = SourceLevels.All;

        }
    }
}