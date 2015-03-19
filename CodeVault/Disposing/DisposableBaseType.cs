using System;
using System.Diagnostics;

namespace Disposing
{
    /// <summary>
    /// Provides supporting infrastructure for implementing the IDisposable interface.
    /// Just inherit this class, then override the Cleanup() method.
    /// http://blogs.msdn.com/b/bclteam/archive/2007/10/30/dispose-pattern-and-object-lifetime-brian-grunkemeyer.aspx
    /// </summary>
    public class DisposableBaseType : IDisposable
    {
        /// <summary>
        /// Returns true if the disposed flag is true, i.e. Dispose()
        /// has been called.
        /// </summary>
        protected bool Disposed
        {
            get
            {
                lock (this)
                {
                    return _disposed;
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool _disposed;

        #region IDisposable Members
        /// <summary>
        /// Disposes the object by calling the virtual Cleanup()
        /// methods then setting the Disposed flag to true.
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (_disposed == false)
                {
                    Cleanup();
                    _disposed = true;

                    GC.SuppressFinalize(this);
                }
            }
        }
        #endregion

        /// <summary>
        /// Override this method to provide your own cleanup code,
        /// e.g. disposing of resources.
        /// </summary>
        protected virtual void Cleanup()
        {
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="DisposableBaseType"/> is reclaimed by garbage collection.
        /// </summary>
        ~DisposableBaseType()
        {
            Cleanup();
        }
    }
}
