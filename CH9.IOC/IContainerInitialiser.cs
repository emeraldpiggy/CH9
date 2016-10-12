using SimpleInjector;

namespace CH9.IOC
{
    public interface IContainerInitialiser
    {
        void ConfigureContainerRegistrations(Container container, RegistrationFilter registrationFilter);
    }
}