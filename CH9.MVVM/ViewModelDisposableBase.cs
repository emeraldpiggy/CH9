using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH9.Framework.Logging;

namespace CH9.MVVM
{
    public abstract class ViewModelDisposableBase<T>:IDisposable
    {
        protected T Model { get; set; }
        private readonly IEnumerable<IDisposable> _disposables;

        protected ViewModelDisposableBase()
        {
            _disposables = new List<IDisposable>();
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
            if (disposing && _disposables != null)
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
                if (_disposables.Any())
                {
                    _disposables.ForEach(d=>d.Dispose());
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
