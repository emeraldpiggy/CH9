using System;
using System.Linq;
using Microsoft.AspNet.SignalR.Client;
using TowersPerrin.RiskAgility.Framework.Configuration;
using TowersPerrin.RiskAgility.Framework.Diagnostics.Logging.Interfaces;

namespace TowersPerrin.RiskAgility.Framework.PushingServices
{
    public abstract class HubClientBase: IDisposable
    {
        protected HubConnection _hubConnection;
        protected IHubProxy _hubProxy;

        protected void Create(string pushingServiceConfigKey, string hubName)
        {
            if (Connected)
                return;

            var pushingServiceConfig = ConfigurationFactory.GetConfigurationManager().ReadApplicationParameter(pushingServiceConfigKey);
            var url = PushingServiceconfigReader.GetPushingServiceUrl(pushingServiceConfig);

            _hubConnection = new HubConnection(url);
            _hubConnection.Error += _hubConnection_Error;
            _hubConnection.Closed += _hubConnection_Closed;
            _hubProxy = _hubConnection.CreateHubProxy(hubName);

            _hubConnection.Start().Wait();
        }

        void _hubConnection_Closed()
        {
            // in case of reconnection timeout, start the connection immediately
            _hubConnection.Start();
        }

        void _hubConnection_Error(Exception ex)
        {
            OnHubConnectionError(ex);
        }

        protected void Create(string pushingServiceConfigKey, string hubName, IRaLog log)
        {
            try
            {
                Create(pushingServiceConfigKey, hubName);
            }
            catch (AggregateException ex)
            {
                LogAggregateException(ex, log);
            }
            catch (Exception ex)
            {
                log.Fatal(ex);
            }
        }

        protected void LogAggregateException(AggregateException ex, IRaLog log)
        {
            var firstException = ex.Flatten().InnerExceptions.FirstOrDefault();
            if (firstException != null)
            {
                log.Error(firstException);
            }
        }

        protected void LogAggregateExceptionAsWarning(AggregateException ex, IRaLog log)
        {
            var firstException = ex.Flatten().InnerExceptions.FirstOrDefault();
            if (firstException != null)
            {
                log.Warn(firstException);
            }
        }

        protected virtual void OnHubConnectionError(Exception ex)
        {}

        protected bool Connected
        {
            get { return _hubConnection != null; }
        }

        public void Dispose()
        {
            if (_hubConnection!= null)
            { 
                _hubConnection.Error -= _hubConnection_Error;
                _hubConnection.Dispose();
            }
        }
    }
}