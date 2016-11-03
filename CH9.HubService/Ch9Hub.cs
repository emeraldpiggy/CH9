using System;
using CH9.Repository.Entity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace CH9.HubService
{
    [HubName("CHub")]
    public class CHub : Hub
    {
        public void BroadcastViewModel(CleaningHouseModel chModel)
        {
            LogMessage(chModel);
            Clients.All.addMessage(chModel);
        }

        public CleaningHouseModel Send(CleaningHouseModel chModel)
        {
            LogMessage(chModel);
            BroadcastViewModel(chModel);
            return chModel;
        }

        private void LogMessage(CleaningHouseModel chModel)
        {
            Console.WriteLine("CleaningHouse Model, 'Dusting' {0}, 'Mopping' {1} ,'Vacumming' {2}", chModel.Dusting, chModel.Mopping, chModel.Vacumming);
        }
    }
}