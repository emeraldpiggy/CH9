using System.ComponentModel;

namespace CH9.Shell.Base
{
    public interface IViewModel<T> : INotifyPropertyChanged
    {
        T Model { get; }
    }
}