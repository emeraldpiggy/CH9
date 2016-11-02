using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CH9.NotificationService
{
    public class Ch9Hub
    {
        private void StartPushingService()
        {
            var address = "http://localhost:8808";
            //_signalR = WebApp.Start<Startup>(address);
        }


        protected void StopServiceHosts()
        {
            //if (_signalR != null)
            //    _signalR.Dispose();
        }

    }

}

