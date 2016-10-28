using System;

namespace TowersPerrin.RiskAgility.Framework.PushingServices
{
    public interface IJobEventNotificationHubClient: IDisposable
    {
        void ConnectHubs(string pushingServiceConfigKey);

        event EventHandler<Exception> Error;
        event EventHandler<JobStartedInfo> JobStarted;
        event EventHandler<JobSubmissionInfo> JobSubmitted;
        event EventHandler<JobCancelRequestedInfo> JobCancelRequested;
        event EventHandler<JobInfo> JobSubmitFailure;
        event EventHandler<JobRescheduledInfo> JobRescheduled;
        event EventHandler<JobInfo> JobFinished;
    }
}