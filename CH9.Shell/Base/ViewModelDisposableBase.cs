using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CH9.Framework;
using CH9.Repository.Annotations;

namespace CH9.Shell.Base
{
    public abstract class ViewModelDisposableBase<T>:IViewModel<T>,IDisposable
    {
        public T Model { get; set; }
        protected readonly IList<IDisposable> Disposables; 

        protected ViewModelDisposableBase()
        {
            Disposables = new List<IDisposable>();
        }

        ~ViewModelDisposableBase()
        {
            Dispose(false);
        }

        [Browsable(false)]
        public bool IsDisposed { get; private set; }


        protected bool IsDisposing { get; private set; }

        private void Dispose(bool disposing)
        {
            if (IsDisposed || IsDisposing)
                return;

            IsDisposing = true;
            OnDispose(disposing);
            if (disposing && Disposables != null)
            {
              
            }
            IsDisposed = true;
            IsDisposing = false;
        }

        /// <summary>
        /// Dispose has been called
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (Disposables.Any())
                {
                    Disposables.ForEach(d=>d.Dispose());
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
