using System;
using System.IO;
using CH9.Repository.Entity;
using Microsoft.AspNet.SignalR.Client;

namespace CH9.Shell.Client
{
    public class Client
    {
        private readonly TextWriter _traceWriter;

        public Client(TextWriter traceWriter)
        {
            _traceWriter = traceWriter;
        }

        public void Run(string url, Action<CleaningHouseModel> updateVm, CleaningHouseModel vm)
        {
            try
            {
                RunHubConnectionApi(url, updateVm, vm);
            }
            catch (Exception exception)
            {
                _traceWriter.WriteLine("Exception: {0}", exception);
            }
        }

        private void RunHubConnectionApi(string url, Action<CleaningHouseModel> updateVm, CleaningHouseModel vm)
        {
            var hubConnection = new HubConnection(url) {TraceWriter = _traceWriter};

            var hubProxy = hubConnection.CreateHubProxy("CHub");
            
            hubProxy.On("addMessage", updateVm);

            hubConnection.Start().Wait();

            hubProxy.Invoke<CleaningHouseModel>("Send", vm).Wait();
        }
    }
}