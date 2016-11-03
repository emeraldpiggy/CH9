using System;
using System.IO;
using System.Windows.Threading;
using CH9.Repository.Entity;
using Microsoft.AspNet.SignalR.Client;

namespace CH9.Shell.Client
{
    public class Client
    {
        private readonly TextWriter _traceWriter;
        private IHubProxy _hubProxy;
        public Client(TextWriter traceWriter)
        {
            _traceWriter = traceWriter;
        }

        public void Run(string url, Action<CleaningHouseModel> updateVm)
        {
            try
            {
                RunHubConnectionApi(url, updateVm);
            }
            catch (Exception exception)
            {
                _traceWriter.WriteLine("Exception: {0}", exception);
            }
        }

        private void RunHubConnectionApi(string url, Action<CleaningHouseModel> updateVm)
        {
            var hubConnection = new HubConnection(url) {TraceWriter = _traceWriter};

            _hubProxy = hubConnection.CreateHubProxy("CHub");
            
            _hubProxy.On("UpdateModel",updateVm);

            hubConnection.Start().Wait();

        }

        public void SendMessage(CleaningHouseModel vm)
        {
            _hubProxy.Invoke<CleaningHouseModel>("Send", vm).Wait();
        }
    }
}