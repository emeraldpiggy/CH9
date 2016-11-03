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

            //app.Map("/raw-connection", map =>
            //{
            //    // Turns cors support on allowing everything
            //    // In real applications, the origins should be locked down
            //    map.UseCors(CorsOptions.AllowAll)
            //       .RunSignalR<RawConnection>();
            //});

            //app.Map("/signalr", map =>
            //{
            //    var config = new HubConfiguration
            //    {
            //        // You can enable JSONP by uncommenting this line
            //        // JSONP requests are insecure but some older browsers (and some
            //        // versions of IE) require JSONP to work cross domain
            //        // EnableJSONP = true
            //    };

            //    // Turns cors support on allowing everything
            //    // In real applications, the origins should be locked down
            //    map.UseCors(CorsOptions.AllowAll)
            //       .RunSignalR(config);
            //});

            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();

            // Turn tracing on programmatically
            GlobalHost.TraceManager.Switch.Level = SourceLevels.Information;
            
        }
    }
}