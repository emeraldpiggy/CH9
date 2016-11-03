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

        public void Run(string url, CleaningHouseModel vm)
        {
            try
            {
                RunHubConnectionApi(url, vm);
            }
            catch (Exception exception)
            {
                _traceWriter.WriteLine("Exception: {0}", exception);
            }
        }

        private void RunHubConnectionApi(string url, CleaningHouseModel vm)
        {
            var hubConnection = new HubConnection(url) {TraceWriter = _traceWriter};

            var hubProxy = hubConnection.CreateHubProxy("CHub");
            hubProxy.On<CleaningHouseModel>("UpdateModel", (chVm) =>
            {
                vm = chVm;
            });

            hubConnection.Start().Wait();

            //hubProxy.Invoke("UpdateModel", vm).Wait();
        }
    }
}