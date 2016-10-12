using System.ComponentModel;

namespace CH9.MVVM
{
    public interface IVisualState : INotifyPropertyChanged
    {
        string VisualState { get; set; }

    }
}