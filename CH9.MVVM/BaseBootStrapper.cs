using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using SimpleInjector;
using LogManager = Caliburn.Micro.LogManager;

namespace CH9.MVVM
{

    public abstract class BaseBootStrapper : BootstrapperBase
    {
        private readonly bool _useApplication;
        private Container _container;

        internal Container Container { get { return _container; } }
        public event EventHandler<EventArgs> Loaded;

        protected BaseBootStrapper(bool useApplication)
            : base(useApplication)
        {
            _useApplication = useApplication;
        }
        protected override void Configure()
        {
            ViewResolver.Initialise();


            try
            {
                _container = SimpleInjectorContainerFactory.CreateProcessScopedContainer();
            }
            catch (Exception ex)
            {
                this.LogFatal(ex);
                throw;
            }

            Root.Container = _container;
            _container.RegisterSingle<IWindowManager>(new CaliburnWindowManager());

            this.LogDebug("Finished Configuring container");

            //when loaded in vsis there is a null instance in the assembly source this must be removed, most like due to the process not being a .net exe
            if (AssemblySource.Instance.Any(a => a == null))
            {
                AssemblySource.Instance.RemoveRange(AssemblySource.Instance.Where(a => a == null).ToArray());
            }

            //Load assemblies into caliburn for view resolution (can probalby optimise the assemblies that are loaded here)
            AssemblySource.Instance.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("TW.")));

            // Do this here since Configure is the only override available if Caliburn is not set to use Application
            if (Application == null)
            {
                InitializeShell();
            }

            LogManager.GetLog = t => new CaliburnLog();

            LoadingMessage(string.Format("{0}", "Configure Complete"));

        }

        protected void InitializeShell()
        {
            foreach (var shellInit in GetAllInstances(typeof(IShellInitialise)).AsNullSafe().OfType<IShellInitialise>())
            {
                shellInit.Initialise(Container);
            }
        }

        protected override void PrepareApplication()
        {
            Application.Startup += HandleStartup;
            Application.DispatcherUnhandledException += OnUnhandledException;
            Application.Exit += OnExit;
        }

        protected override void StartDesignTime()
        {
            base.StartDesignTime();
            if (!_useApplication)
            {
                FireLoaded();
            }
        }

        private void HandleStartup(object sender, StartupEventArgs e)
        {
            OnStartup(sender, e);
            FireLoaded();
        }

        protected void OnLoaded(Container container)
        {

        }

        private void FireLoaded()
        {
            OnLoaded(Container);
            var handler = Loaded;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }



        protected override void OnExit(object sender, EventArgs e)
        {
            base.OnExit(sender, e);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            return _container.GetInstance(serviceType);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }
    }


    public class ContainerBootStrapper : BaseBootStrapper
    {
        private readonly bool _launchIShell;

        public ContainerBootStrapper(bool launchIShell, bool useApplication, Action<Container> containerConfigured, Action<Container> completeAction)
            : this(launchIShell, useApplication)
        {
            Loaded += (o, e) => completeAction(Container);
            containerConfigured(Container);
        }

        public ContainerBootStrapper(bool launchIShell, bool useApplication = true)
            : base(useApplication)
        {
            _launchIShell = launchIShell;
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            LoadingMessage("Loading Shell");
            if (Container == null)
            {
                this.LogFatal("Error Container has not been configured!");
                return;
            }

            InitializeShell();

            IWindowManagerService windowManager;
            try
            {
                windowManager = GetInstance(typeof(IWindowManagerService), null) as IWindowManagerService;
                if (windowManager == null)
                    throw new NullReferenceException("Cannot load window manager service");
            }
            catch
            {
                throw new Exception("Cannot load window manager service");
            }

            if (_launchIShell)
            {
                windowManager.ShowWindow(GetInstance(typeof(IShell), null));
            }
        }
    }
}
}
