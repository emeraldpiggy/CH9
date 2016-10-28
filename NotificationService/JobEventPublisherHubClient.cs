using System;
using System.Threading.Tasks;
using TowersPerrin.RiskAgility.Framework.Business.ComponentFramework.Common.Constants;
using TowersPerrin.RiskAgility.Framework.Configuration;
using TowersPerrin.RiskAgility.Framework.Diagnostics.Logging.Interfaces;
using TowersPerrin.RiskAgility.Framework.PushingServices;

namespace TowersPerrin.RiskAgility.Framework.Business.JobManagement
{
    [ContainerRegistration(LifetimeManagement = IocLifestyle.Singleton)]
    public class JobEventPublisherHubClient : HubClientBase, IJobEventPublisherHubClient
    {
        private IRaLog _log;

        public void ConnectHubs(IRaLog log)
        {
            _log = log;
            Create(ConfigurationParameterNames.PushingService, PushingServiceConstants.JobMonitorHub, log);
        }

        
        public async Task PublishJobSubmittedAsync(JobSubmissionInfo jobInfo)
        {
            try
            {
                if (_hubProxy == null)
                    throw new InvalidOperationException("hubProxy has not been created yet.");

                await _hubProxy.Invoke(PushingServiceConstants.JobSubmitted, jobInfo);
            }
            catch (AggregateException ex)
            {
                LogAggregateException(ex, _log);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        public async Task PublishJobStartedAsync(JobStartedInfo jobInfo)
        {
            try
            {
                if (_hubProxy == null)
                    throw new InvalidOperationException("hubProxy has not been created yet.");

                await _hubProxy.Invoke(PushingServiceConstants.JobStarted, jobInfo);
            }
            catch (AggregateException ex)
            {
                LogAggregateException(ex, _log);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        public async Task PublishJobCancelRequestedAsync(JobCancelRequestedInfo jobInfo)
        {
            try
            {
                if (_hubProxy == null)
                    throw new InvalidOperationException("hubProxy has not been created yet.");

                await _hubProxy.Invoke(PushingServiceConstants.JobCancelRequested, jobInfo);
            }
            catch (AggregateException ex)
            {
                LogAggregateException(ex, _log);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        public async Task PublishJobSubmitFailureAsync(JobInfo jobInfo)
        {
            try
            {
                if (_hubProxy == null)
                    throw new InvalidOperationException("hubProxy has not been created yet.");

                await _hubProxy.Invoke(PushingServiceConstants.JobSubmitFailure, jobInfo);
            }
            catch (AggregateException ex)
            {
                LogAggregateException(ex, _log);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        public async Task PublishJobRescheduledAsync(JobRescheduledInfo jobInfo)
        {
            try
            {
                if (_hubProxy == null)
                    throw new InvalidOperationException("hubProxy has not been created yet.");

                await _hubProxy.Invoke(PushingServiceConstants.JobRescheduled, jobInfo);
            }
            catch (AggregateException ex)
            {
                LogAggregateException(ex, _log);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }
        
        public async Task PublishJobFinishedAsync(JobInfo jobInfo)
        {
            try
            {
                if (_hubProxy == null)
                    throw new InvalidOperationException("hubProxy has not been created yet.");

                await _hubProxy.Invoke(PushingServiceConstants.JobFinished, jobInfo);
            }
            catch (AggregateException ex)
            {
                LogAggregateException(ex, _log);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        protected override void OnHubConnectionError(Exception ex)
        {
            _log.Error(ex);
        }
    }
}