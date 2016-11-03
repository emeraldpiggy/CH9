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
            LogMessage("BroadCast ViewModel", chModel);
            Clients.All.UpdateModel(chModel);
        }

        public CleaningHouseModel Send(CleaningHouseModel chModel)
        {
            LogMessage("Receive Message",chModel);
            BroadcastViewModel(chModel);
            return chModel;
        }

        private void LogMessage(string caller,CleaningHouseModel chModel)
        {
            Console.WriteLine("{3}, 'Dusting' {0}, 'Mopping' {1} ,'Vacumming' {2}", chModel.Dusting, chModel.Mopping, chModel.Vacumming, caller);
        }
    }
}