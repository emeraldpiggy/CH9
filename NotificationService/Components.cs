using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Owin.Hosting;
using TowersPerrin.RiskAgility.Framework.Business.ComponentFramework;
using TowersPerrin.RiskAgility.Framework.Business.ComponentFramework.Common.Constants;
using TowersPerrin.RiskAgility.Framework.Business.ComponentFramework.Hosting;
using TowersPerrin.RiskAgility.Framework.Business.JobManagement;
using TowersPerrin.RiskAgility.Framework.Configuration;
using TowersPerrin.RiskAgility.Framework.Configuration.Implementations;
using TowersPerrin.RiskAgility.Framework.DAL.DataManagement;
using TowersPerrin.RiskAgility.Framework.Diagnostics.Logging;
using TowersPerrin.RiskAgility.Framework.Diagnostics.Logging.Interfaces;
using TowersPerrin.RiskAgility.Framework.PushingServices;
using TowersPerrin.RiskAgility.Framework.Resources.Business;

namespace TowersPerrin.RiskAgility.Framework.Business.Administration.Service
{
    [ServiceDescriptor("AdministrationService")]
    public class Components : MultiHostingComponent
    {
        private static int sleepTimeBetweenJobLaunchesInMillisecs = 1000;
        private static readonly TimeSpan SleepTimeBetweenTerminatedJobsHandling = new TimeSpan(0, 1, 0);
        private IDisposable _signalR;

        public Components()
            : base(
            new[]
            {
                new HostingDescriptor(typeof(IHealthAgent)),
                new HostingDescriptor(typeof(IConfigurationParameterService)),
                new HostingDescriptor(typeof(IAuditLogService)),
                new HostingDescriptor(typeof(ISystemLogService))
            }
            )
        {
            sleepTimeBetweenJobLaunchesInMillisecs = Convert.ToInt32(ConfigurationFactory.GetConfigurationManager()
                .ReadApplicationParameter(ConfigurationParameterNames.SleepTimeBetweenJobLaunchesInMillisecs));
            IRaLog log = null;
            try
            {
                var configurationManager = ConfigurationFactory.GetConfigurationManager();
                var container = configurationManager.GetContainer();

                log = RaLogFactory.Create("AdministrationManager", container);

                var pushingServiceConfig = configurationManager.ReadApplicationParameter(ConfigurationParameterNames.PushingService);
                StartPushingService(pushingServiceConfig);

                StartJobDispatching(container, log);
                StartTerminatedJobsHandling(container, log);
                StartBackgroundWorkers(container, log);
            }
            catch (Exception ex)
            {
                if (log != null)
                    log.Fatal(ex, "Fatal error starting AdministrationManager");

                Environment.FailFast(ex.Message);
            }
        }

        private void StartPushingService(string pushingServiceConfig)
        {
            var address = PushingServiceconfigReader.GetPushingServiceUrl(pushingServiceConfig);
            _signalR = WebApp.Start<Startup>(address); 
        }

        protected override void OnHostContainerResolvingComponent(object sender, ComponentServiceHostContainerEventArgs e)
        {
        }

        protected override void OnComponentServiceHostOpening(object sender, ComponentServiceHostEventArgs e)
        {
        }

        protected override void OnComponentServiceHostOpened(object sender, ComponentServiceHostEventArgs e)
        {
        }

        private static void StartBackgroundWorkers(IRAContainer container, IRaLog log)
        {
            log.Info("Starting Background Workers in AdministrationManagerService at: " + DateTime.UtcNow + " UTC");
            var tuple = new Tuple<IRAContainer, IRaLog>(container, log);
            new Thread(PurgeOperations)
                {
                    IsBackground = true
                }
                .Start(tuple);
            log.Info("Started Background Workers in AdministrationManagerService at: " + DateTime.UtcNow + " UTC");
        }

        protected override void StopServiceHosts()
        {
            base.StopServiceHosts();
            if (_signalR != null)
                _signalR.Dispose();
        }

        #region Job dispatching periodic activity

        private static void StartJobDispatching(IRAContainer container, IRaLog log)
        {
            log.Info("Starting job queue management in Administration Service at: " + DateTime.UtcNow + " UTC");
            var containerAndLog = new Tuple<IRAContainer, IRaLog>(container, log);
            new Thread(DispatchJobs)
                {
                    IsBackground = true
                }.Start(containerAndLog);
            log.Info("Started job queue management in Administration Service at: " + DateTime.UtcNow + " UTC");
        }

        private static void DispatchJobs(object containerAndLogObj)
        {
            if (!(containerAndLogObj is Tuple<IRAContainer, IRaLog>))
                return;
            var containerAndLog = (Tuple<IRAContainer, IRaLog>)containerAndLogObj;
            var container = containerAndLog.Item1;
            var log = containerAndLog.Item2;
            IJobManager jobManager = null;
            while (true)
            {
                try
                {
                    jobManager = container.Resolve<IJobManager>();
                    jobManager.DispatchJobs();
                }
                catch (Exception e)
                {
                    log.Error(new JobManagerException(Messages.RAF4084(), e));
                }
                finally
                {
                    try
                    {
                        if (jobManager != null)
                            container.Release(jobManager);
                    }
                    catch
                    {
                    }
                }
                Thread.Sleep(sleepTimeBetweenJobLaunchesInMillisecs);
            }
        }

        #endregion Job dispatching periodic activity

        #region termintaed jobs handling activity

        private static void StartTerminatedJobsHandling(IRAContainer container, IRaLog log)
        {
            log.Info("Starting job queue management in Administration Service at: " + DateTime.UtcNow + " UTC");
            var containerAndLog = new Tuple<IRAContainer, IRaLog>(container, log);
            new Thread(HandleTerminatedJobs)
                {
                    IsBackground = true
                }.Start(containerAndLog);
            log.Info("Started job queue management in Administration Service at: " + DateTime.UtcNow + " UTC");
        }

        private static void HandleTerminatedJobs(object containerAndLogObj)
        {
            if (!(containerAndLogObj is Tuple<IRAContainer, IRaLog>))
                return;
            var containerAndLog = (Tuple<IRAContainer, IRaLog>)containerAndLogObj;
            var container = containerAndLog.Item1;
            var log = containerAndLog.Item2;
            IJobManager jobManager = null;
            while (true)
            {
                try
                {
                    jobManager = container.Resolve<IJobManager>();
                    jobManager.HandleTerminatedJobs();
                }
                catch (Exception e)
                {
                    log.Error(new JobManagerException(Messages.RAF4084(), e));
                }
                finally
                {
                    try
                    {
                        if (jobManager != null)
                            container.Release(jobManager);
                    }
                    catch
                    {
                    }
                }
                Thread.Sleep(SleepTimeBetweenTerminatedJobsHandling);
            }
        }

        #endregion termintaed jobs handling activity

        #region DB purging activity
        //TODO - refactor to business class

        private static void PurgeOperations(object tuple)
        {
            const int fiveMinutes = 300000;
            const int tenMinutes = 600000;

            if (!(tuple is Tuple<IRAContainer, IRaLog>))
                return;

            var containerAndLog = (Tuple<IRAContainer, IRaLog>)tuple;

            var container = containerAndLog.Item1;
            var log = containerAndLog.Item2;

            while (true)
            {
                PurgeDatapackOrphans(container, log);
                Thread.Sleep(fiveMinutes);
                
                PurgeDatapackOrphans(container, log);
                Thread.Sleep(fiveMinutes);
                
                PreparePendingFlushJobOutputDatapacks(container, log);
                Thread.Sleep(tenMinutes);

                FlushJobOutputDatapacks(container, log);
                Thread.Sleep(tenMinutes);
            }
        }

        private static void FlushJobOutputDatapacks(IRAContainer container, IRaLog log)
        {
            IJobManager jobManager = null;

            try
            {
                jobManager = container.Resolve<IJobManager>();

                jobManager.FlushOutputDatapacksWithChildren();
            }
            catch (Exception ex)
            {
                if (log != null)
                {
                    log.Error(ex, "Error jobManger.FlushOutputDatapacksWithChildren");
                }
            }
            finally
            {
                try
                {
                    if (jobManager != null)
                        container.Release(jobManager);
                }
                catch
                {
                }
            }
        }

        private static void PreparePendingFlushJobOutputDatapacks(IRAContainer container, IRaLog log)
        {
            IDataManager dataManager = null;
            try
            {
                dataManager = container.Resolve<IDataManager>();

                var versions = dataManager.PreparePendingFlushDatapackVersions();

                dataManager.FlushDatapackVersions(versions);
            }
            catch (Exception ex)
            {
                if (log != null)
                {
                    log.Error(ex, "Error jobManger.FlushOutputDatapacksWithChildren");
                }
            }
            finally
            {
                try
                {
                    container.Release(dataManager);
                }
                catch
                {
                }
            }
        }

        private static void PurgeDatapackOrphans(IRAContainer container, IRaLog log)
        {
            IDataManager dataManager = null;
            try
            {
                dataManager = container.Resolve<IDataManager>();

                dataManager.PurgeDatapackOrphans();
            }
            catch (Exception ex)
            {
                if (log != null)
                {
                    log.Error(ex, "Error jobManger.FlushOutputDatapacksWithChildren");
                }
            }
            finally
            {
                try
                {
                    container.Release(dataManager);
                }
                catch
                {
                }
            }
        }

        #endregion  DB purging activity
    }
}
