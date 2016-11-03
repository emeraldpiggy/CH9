using System;
using CH9.Repository.Entity;

namespace CH9.Shell.Client
{
    public class HubClient
    {
        private Client _client;
        public void SetupHubProxy(Action<CleaningHouseModel> updateVm)
        {
            string url = @"http://localhost:8088/Ch9";
            var writer = Console.Out;
            _client = new Client(writer);
            _client.Run(url, updateVm);
        }

        public void SendMessage(CleaningHouseModel vm)
        {
            _client.SendMessage(vm);
        }
    }
}
