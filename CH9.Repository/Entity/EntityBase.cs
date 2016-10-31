using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CH9.Repository.Annotations;

namespace CH9.Repository.Entity
{
    public abstract class EntityBase:IEntity
    {
        public EntityStatus Status { get; set; }

        public Guid EntityId { get; set; }



        public void RaisePropertyChanged(string propertyName, bool setHasChangedFlag = false)
        {
            // just checking the EntityStatus
            if (Status != EntityStatus.Loading)
            {
                OnPropertyChanged(propertyName);  
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}