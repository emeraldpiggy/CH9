using SimpleInjector;
using Container = SimpleInjector.Container;

namespace CH9.IOC
{
    public class SimpleInjectorContainerFactory
    {
        /// <summary>
        /// Create a process scoped container given the supplied 
        /// </summary>
        public static Container CreateProcessScopedContainer(IContainerInitialiser containerInitialiser)
        {
            var container = new Container();
            //Allow overriding so the corepackage can be injected manually
            container.Options.AllowOverridingRegistrations = true;
            var registrationFilter = new RegistrationFilter
            {
                RegisterNonProcessScoped = true,
                RegisterProcessScoped = true,
                RegisterShared = true,
                RegisterTransient = true
            };
            containerInitialiser.ConfigureContainerRegistrations(container, registrationFilter);
            container.RegisterSingleton(containerInitialiser);
            //This is to prevent automatic creation of unregistered concrete types.
            container.DisableAutomaticRegistrationForConcreteTypes();
            return container;
        }
    }

    public class DefaultEmptyContainerInitialiser : IContainerInitialiser
    {
        public void ConfigureContainerRegistrations(Container container, RegistrationFilter registrationFilter)
        {
        }
    }
}
