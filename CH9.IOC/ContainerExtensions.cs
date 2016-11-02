using System;
using System.Collections.Generic;
using System.Linq;
using SimpleInjector;
using Container = SimpleInjector.Container;

namespace CH9.IOC
{
    public static class ContainerExtensions
    {
        public static IEnumerable<Type> GetRegisteredInterfacesForConcreteType(this Container container, Type concreteType)
        {
            var types = container.GetCurrentRegistrations()
                    .Where(r => r.Registration.ImplementationType == concreteType)
                    .Select(r => r.ServiceType);

            var containerAsNested = container as INestedContainer;

            if (containerAsNested != null)
            {
                types = types.Union(containerAsNested.ParentContainer.GetRegisteredInterfacesForConcreteType(concreteType));
            }

            return types;
        }

        public static InstanceProducer GetInstanceProducerForServiceType(this Container container, Type serviceType)
        {
            var producer = container.GetCurrentRegistrations().FirstOrDefault(r => r.ServiceType == serviceType);

            if (producer == null)
            {

                var containerAsNested = container as INestedContainer;

                if (containerAsNested != null)
                {
                    producer = containerAsNested.ParentContainer.GetInstanceProducerForServiceType(serviceType);
                }
            }

            return producer;
        }

        /// <summary>
        /// Registers the instance used types defined in Export, ExportScope, InheritedExport and InheritiedExportScope attributes
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="instance">The instance.</param>
        public static void RegisterSingletonInstance(this Container container, object instance)
        {
            var type = instance.GetType();
            container.RegisterSingleton(type, instance);
        }

        /// <summary>
        /// Prevent automatic creation of concrete types if they're not explictly registered (by default simple injector will create them)
        /// </summary>
        /// <param name="container"></param>
        public static void DisableAutomaticRegistrationForConcreteTypes(this Container container)
        {
            container.ResolveUnregisteredType += (sender, args) =>
            {
                if (!args.Handled && !args.UnregisteredServiceType.IsAbstract)
                {
                    //throw new ActivationException($"{args.UnregisteredServiceType}");
                }
            };
        }
    }

}
