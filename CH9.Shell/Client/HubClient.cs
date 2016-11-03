using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CH9.Shell.Client
{
    public class HubClient
    {
        public void SetupHubProxy()
        {
            string url = @"http://localhost:8088/Ch9";
            var writer = Console.Out;
            var client = new Client(writer);
            client.Run(url);

        }
    }
}
