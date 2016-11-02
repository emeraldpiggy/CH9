using System;
using System.IO;
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

        public void Run(string url)
        {
            try
            {
                RunHubConnectionApi(url);
            }
            catch (Exception exception)
            {
                _traceWriter.WriteLine("Exception: {0}", exception);
                throw;
            }
        }

        private void RunHubConnectionApi(string url)
        {
            var hubConnection = new HubConnection(url) {TraceWriter = _traceWriter};

            var hubProxy = hubConnection.CreateHubProxy("Ch9Hub");
            hubProxy.On<string>("displayMessage", (data) => hubConnection.TraceWriter.WriteLine(data));

            hubConnection.Start().Wait();
            hubConnection.TraceWriter.WriteLine("transport.Name={0}", hubConnection.Transport.Name);

            hubProxy.Invoke("DisplayMessageCaller", "Hello Caller!").Wait();

            string joinGroupResponse = hubProxy.Invoke<string>("JoinGroup", hubConnection.ConnectionId, "CommonClientGroup").Result;
            hubConnection.TraceWriter.WriteLine("joinGroupResponse={0}", joinGroupResponse);

            hubProxy.Invoke("DisplayMessageGroup", "CommonClientGroup", "Hello Group Members!").Wait();

            string leaveGroupResponse = hubProxy.Invoke<string>("LeaveGroup", hubConnection.ConnectionId, "CommonClientGroup").Result;
            hubConnection.TraceWriter.WriteLine("leaveGroupResponse={0}", leaveGroupResponse);

            hubProxy.Invoke("DisplayMessageGroup", "CommonClientGroup", "Hello Group Members! (caller should not see this message)").Wait();

            hubProxy.Invoke("DisplayMessageCaller", "Hello Caller again!").Wait();
        }
    }
}