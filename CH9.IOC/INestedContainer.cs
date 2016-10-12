using SimpleInjector;

namespace CH9.IOC
{
    public interface INestedContainer
    {
        Container ParentContainer { get; }
    }
}