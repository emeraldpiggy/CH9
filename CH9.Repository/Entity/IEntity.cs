using System.ComponentModel;

namespace CH9.Repository.Entity
{
    public interface IEntity: INotifyPropertyChanged
    {
        EntityStatus Status { get; set; }
    }
}