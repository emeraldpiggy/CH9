using System.ComponentModel;

namespace CH9.MVVM
{
    public interface IViewModel<T> : INotifyPropertyChanged
    {
        T Model { get; }
    }
}