using System;
using Microsoft.AspNet.SignalR.Client;
using TowersPerrin.RiskAgility.Framework.Configuration;

namespace TowersPerrin.RiskAgility.Framework.PushingServices
{
    [ContainerRegistration(LifetimeManagement = IocLifestyle.Transient)]
    public class JobEventNotificationHubClient : HubClientBase, IJobEventNotificationHubClient
    {
        public void ConnectHubs(string pushingServiceConfigKey)
        {
            if (Connected)
                return;

            Create(pushingServiceConfigKey, PushingServiceConstants.JobMonitorHub);

            _hubProxy.On<JobStartedInfo>(PushingServiceConstants.JobStarted, WhenJobStarted);
            _hubProxy.On<JobSubmissionInfo>(PushingServiceConstants.JobSubmitted, WhenJobSubmitted);
            _hubProxy.On<JobCancelRequestedInfo>(PushingServiceConstants.JobCancelRequested, WhenJobCancelRequested);
            _hubProxy.On<JobInfo>(PushingServiceConstants.JobSubmitFailure, WhenJobSubmitFailure);
            _hubProxy.On<JobRescheduledInfo>(PushingServiceConstants.JobRescheduled, WhenJobRescheduled);
            _hubProxy.On<JobInfo>(PushingServiceConstants.JobFinished, WhenJobFinished);
        }
        
        public event EventHandler<Exception> Error;
        public event EventHandler<JobStartedInfo> JobStarted;
        public event EventHandler<JobSubmissionInfo> JobSubmitted;
        public event EventHandler<JobCancelRequestedInfo> JobCancelRequested;
        public event EventHandler<JobInfo> JobSubmitFailure;
        public event EventHandler<JobRescheduledInfo> JobRescheduled;
        public event EventHandler<JobInfo> JobFinished;

        protected override void OnHubConnectionError(Exception ex)
        {
            OnError(ex);
        }

        private void WhenJobSubmitted(JobSubmissionInfo job)
        {
            if (job != null)
                OnJobSubmitted(job);
        }

        private void WhenJobStarted(JobStartedInfo job)
        {
            if (job != null)
                OnJobStarted(job);
        }

        private void WhenJobCancelRequested(JobCancelRequestedInfo job)
        {
            if (job != null)
                OnJobCancelRequested(job);
        }

        private void WhenJobSubmitFailure(JobInfo job)
        {
            if (job != null)
                OnJobSubmitFailure(job);
        }

        private void WhenJobRescheduled(JobRescheduledInfo job)
        {
            if (job != null)
                OnJobRescheduled(job);
        }

        private void WhenJobFinished(JobInfo job)
        {
            if (job!= null)
                OnJobFinished(job);
        }
        
        private void OnError(Exception ex)
        {
            if (Error != null)
                Error(this, ex);
        }

        private void OnJobStarted(JobStartedInfo jobInfo)
        {
            if (JobStarted != null)
            {
                JobStarted(this, jobInfo);
            }
        }

        private void OnJobSubmitted(JobSubmissionInfo jobInfo)
        {
            if (JobSubmitted != null)
            {
                JobSubmitted(this, jobInfo);
            }
        }

        private void OnJobCancelRequested(JobCancelRequestedInfo jobInfo)
        {
            if (JobCancelRequested != null)
            {
                JobCancelRequested(this, jobInfo);
            }
        }

        private void OnJobSubmitFailure(JobInfo jobInfo)
        {
            if (JobSubmitFailure != null)
            {
                JobSubmitFailure(this, jobInfo);
            }
        }

        private void OnJobRescheduled(JobRescheduledInfo jobInfo)
        {
            if (JobRescheduled != null)
            {
                JobRescheduled(this, jobInfo);
            }
        }


        private void OnJobFinished(JobInfo jobInfo)
        {
            if (JobFinished != null)
            {
                JobFinished(this, jobInfo);
            }
        }
    }
}
