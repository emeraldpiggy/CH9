using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace CH9.HubService
{
    [HubName("Ch9Hub")]
    internal class Ch9Hub : Hub
    {
        public void DetermineLength(string message)
        {
            Console.WriteLine(message);

            string newMessage = $@"{message} has a length of: {message.Length}";
            Clients.All.ReceiveLength(newMessage);
        }
    }
}