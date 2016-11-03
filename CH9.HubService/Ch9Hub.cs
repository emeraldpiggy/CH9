using System;
using CH9.Repository.Entity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace CH9.HubService
{
    [HubName("CHub")]
    public class CHub : Hub
    {
        public void DetermineLength(string message)
        {
            Console.WriteLine(message);

            string newMessage = $@"{message} has a length of: {message.Length}";
            Clients.All.ReceiveLength(newMessage);
        }

        public void PublishPropertyChanged(CleaningHouseModel model)
        {
            
        }
    }
}