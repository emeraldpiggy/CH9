using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using Caliburn.Micro;
using CH9.Framework.Logging;
using CH9.IOC;
using CH9.Shell;
using SimpleInjector;

namespace CH9.MVVM
{
    public class Root
    {
        private static Container _container;

        public static Container Container
        {
            get { return _container; }
            set
            {
                if (!Equals(_container, value))
                {
                    _container = value;
                }
            }
        }
    }

    public class Ch9BootStrapper : BootstrapperBase
    {
        private readonly bool _useApplication;
        private Container _container;
        private readonly bool _launchIShell;

        internal Container Container => _container;
        public event EventHandler<EventArgs> Loaded;

        public Ch9BootStrapper()
        {
            _useApplication = true;
            _launchIShell = true;
            Initialize();
        }

        public Ch9BootStrapper(bool useApplication)
                : base(useApplication)
        {
            _useApplication = useApplication;
        }

        protected override void Configure()
        {
            //ViewResolver.Initialise();

            try
            {
                _container = SimpleInjectorContainerFactory.CreateProcessScopedContainer(new DefaultEmptyContainerInitialiser());
            }
            catch (Exception ex)
            {
                Ch9LogManager.Log(GetType(), LogLevel.Debug, ex.ToString());
                throw;
            }

            Root.Container = _container;
            RegisterInstance();



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
        }

        private void RegisterInstance()
        {
            _container.RegisterSingleton<IWindowManager>(new WindowManagerBase());
            _container.Register<IShell, ShellViewModel>();
            _container.Register<IWindowManagerService, WindowManagerBase>(Lifestyle.Transient);
            _container.Register<IShellInitialise, ShellInitialise>(Lifestyle.Transient);
            _container.RegisterCollection<IShellInitialise>();
            _container.Register(typeof(IViewModel<>), new[] { typeof(ViewModelDisposableBase<>).Assembly }, Lifestyle.Transient);


            var allAssemblies = GetAllAssemblies();

            foreach (var allAssembly in allAssemblies)
            {
                var registrations = allAssembly.GetExportedTypes()
                    .Where(t => t.Namespace != null && t.Namespace.StartsWith("CH9") && !t.Namespace.Contains("CH9.MVVM")
                    && !t.Namespace.Contains("CH9.IOC") && !t.Namespace.Contains("CH9.Repository")
                    && t.IsClass && !t.IsGenericType && t.GetInterfaces().Any())
                    .Select(type => new {Service = type.GetInterfaces().First(), Implementation = type}).ToArray();

                foreach (var reg in registrations)
                {
                    _container.Register(reg.Service, reg.Implementation, Lifestyle.Transient);
                }
            }
        }

        private static List<Assembly> GetAllAssemblies()
        {
            List<Assembly> allAssemblies = new List<Assembly>();
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (path != null)
                foreach (string dll in Directory.GetFiles(path, "CH9*.dll"))
                {
                    allAssemblies.Add(Assembly.LoadFile(dll));
                }
            return allAssemblies;
        }

        protected void InitializeShell()
        {
            foreach (var shellInit in GetAllInstances(typeof(IShellInitialise)).OfType<IShellInitialise>())
            {
                shellInit.Initialise(Container);
            }
        }

        protected override void PrepareApplication()
        {
            Application.Startup += HandleStartup;
            Application.DispatcherUnhandledException += OnUnhandledException;
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
            handler?.Invoke(this, EventArgs.Empty);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            return _container.GetInstance(serviceType);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            if (Container == null)
            {
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

    internal class ShellInitialise : IShellInitialise
    {
        #region Implementation of IShellInitialise

        public void Initialise(Container container)
        {
            var identidy = WindowsIdentity.GetCurrent();
            Thread.CurrentPrincipal = new GenericPrincipal(identidy, null);
            AppDomain.CurrentDomain.SetThreadPrincipal(Thread.CurrentPrincipal);
        }
        #endregion
    }


    public interface IShellInitialise
    {
        void Initialise(Container container);
    }
}
