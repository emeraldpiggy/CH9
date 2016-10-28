using System;

using TowersPerrin.RiskAgility.Framework.Common.Helpers;

namespace TowersPerrin.RiskAgility.Framework.PushingServices
{
    public class JobInfo : JobInfoBase
    {
        private DateTime? _actualStartTime;
        private DateTime? _actualEndTime;
        
        #region Public Properties
        
        public DateTime? ActualStartTime
        {
            get { return _actualStartTime; }
            set { _actualStartTime = value.HasValue ? ConvertToUtc(value.Value) : value; }
        }

        public DateTime? ActualEndTime
        {
            get { return _actualEndTime; }
            set { _actualEndTime = value.HasValue ? ConvertToUtc(value.Value) : value; }
        }

        public int FinishedResult { get; set; }

        public int CompletedReason { get; set; }

        public int FailedReason { get; set; }

        #endregion Public Properties
    }

    public class JobSubmissionInfo : JobInfoBase
    {
    }

    public class JobStartedInfo : JobInfoBase
    {
        private DateTime? _actualStartTime;
        public DateTime? ActualStartTime
        {
            get { return _actualStartTime; }
            set { _actualStartTime = value.HasValue ? ConvertToUtc(value.Value) : value; }
        }
    }

    public class JobCancelRequestedInfo : JobInfoBase
    {
    }

    public class JobRescheduledInfo : JobInfoBase
    {
        public virtual DateTime? ScheduledTime
        {
            get { return _scheduledTime; }
            set { _scheduledTime = value.HasValue ? ConvertToUtc(value.Value) : value; }
        }
        private DateTime? _scheduledTime;
    }

    public class JobInfoBase
    {
        private DateTime _submitTime;
        private DateTime? _queuedTime;

        #region Public Properties

        public int ID { get; set; }
        public string Name { get; set; }
        public int ExecutableDatapackId { get; set; }
        public int ProjectID { get; set; }
        public string UserName { get; set; }

        public DateTime SubmitTime
        {
            get { return _submitTime; }
            set { _submitTime = ConvertToUtc(value); }
        }

        public DateTime? QueuedTime
        {
            get { return _queuedTime; }
            set { _queuedTime = value.HasValue ? ConvertToUtc(value.Value) : value; }
        }
        
        #endregion Public Properties

        // Converts the passed time to UTC time. Assumes that DateTimeKind.Unspecified is a UTC time.
        public static DateTime ConvertToUtc(DateTime t)
        {
            return t.ConvertToUtc();
        }
    }

    public class JobProgress
    {
        public JobProgress(int jobId, int percentage)
        {
            JobId = jobId;
            Percentage = percentage;
        }

        public int JobId { get; set; }
        public int Percentage { get; set; }
    }

}
