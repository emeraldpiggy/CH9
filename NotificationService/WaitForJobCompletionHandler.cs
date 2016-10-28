using System;
using System.Threading;
using TowersPerrin.RiskAgility.Framework.Configuration;

namespace TowersPerrin.RiskAgility.Framework.PushingServices
{
    public enum JobCompletionResult
    {
        Completed,
        NotificationError,
        Timeout
    }

    public class JobCompletionInfo
    {
        public JobCompletionInfo()
        {
        }

        public JobCompletionInfo(JobInfo jobInfo)
        {
            JobInfo = jobInfo;
            Result = JobCompletionResult.Completed;
        }

        public JobCompletionInfo(Exception notificationError)
        {
            NotificationError = notificationError;
            Result = JobCompletionResult.NotificationError;
        }

        public JobInfo JobInfo { get; set; }
        public Exception NotificationError { get; set; }
        public JobCompletionResult Result { get; set; }
    }


    [ContainerRegistration(LifetimeManagement = IocLifestyle.Transient)]
    public class WaitForJobCompletionHandle : IWaitForJobCompletionHandle
    {
        private readonly IRAContainer _container;

        /// <summary>
        /// Used to start the pushing service
        /// </summary>
        private const string PushingService = "PushingService";


        public WaitForJobCompletionHandle(IRAContainer container)
        {
            _container = container;
        }


        public JobCompletionInfo WaitForCompletion(int jobId)
        {
            return WaitForCompletionInternal(jobId, false, new TimeSpan());
        }

        public JobCompletionInfo WaitForCompletion(int jobId, TimeSpan timeout)
        {
            return WaitForCompletionInternal(jobId, true, timeout);
        }


        private JobCompletionInfo WaitForCompletionInternal(int jobId, bool hasTimeout, TimeSpan timeout)
        {
            JobCompletionInfo result = null;

            using (var notificationClient = _container.Resolve<IJobEventNotificationHubClient>())
            {
                var waitHandle = new ManualResetEventSlim(false);
                notificationClient.ConnectHubs(PushingService);

                EventHandler<JobInfo> jobFinishedDelegate = (sender, info) =>
                {
                    // if is the same job which listens to
                    if (info.ID == jobId)
                    {
                        result = new JobCompletionInfo(info);
                        waitHandle.Set();
                    }
                };

                EventHandler<Exception> jobErrorDelegate = (sender, ex) =>
                {
                    result = new JobCompletionInfo(ex);
                    waitHandle.Set();
                };

                notificationClient.JobFinished += jobFinishedDelegate;
                notificationClient.Error += jobErrorDelegate;

                try
                {
                    if (hasTimeout)
                        waitHandle.Wait(timeout);
                    else
                        waitHandle.Wait();
                }
                finally
                {
                    notificationClient.JobFinished -= jobFinishedDelegate;
                    notificationClient.Error -= jobErrorDelegate;
                }

                // time out
                if (result == null)
                    result = new JobCompletionInfo {Result = JobCompletionResult.Timeout};
            }

            return result;
        }
    }

    public interface IWaitForJobCompletionHandle
    {
        JobCompletionInfo WaitForCompletion(int jobId);
        JobCompletionInfo WaitForCompletion(int jobId, TimeSpan timeout);
    }
}