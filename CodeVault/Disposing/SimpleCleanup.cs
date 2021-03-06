using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;



namespace Disposing {
    class BaseClass : IDisposable {
        public BaseClass() {
        }

        #region IDisposable (in base class) Implementation
        #region Design Pattern Explanation
        // When do you need to implement IDisposable?
        // ==========================================
        // A1. If your class owns any objects that implement IDisposable then it
        //     must implement IDisposable and Dispose must call Dispose on those objects.
        // A2. If your class owns any unmanaged resource you should wrap the resource in
        //     a class derived from SafeHandle. This effectively converts the unmanaged
        //     resource into a managed resource, then you should follow rule 1 on it.
        // A3. Never write a finalizer. A finalizer is not necessary because of step 2:
        //     SafeHandle implements its own finalizer which ensures the unmanaged resource
        //     will get cleaned up.
        // A4. Call CheckDisposed() at the start of every public method. [Optional: only
        //     do this in methods which need access to the owned resources to succeed.]
        //
        // There are some exceptions to rule A3:
        // http://www.bluebytesoftware.com/blog/default,date,2005-12-29.aspx
        // 
        // IDisposable In Inheritance Hierarchies
        // ======================================
        // B1. In the base class, inherit the IDisposable interface.
        // B2. In the base class, you should hold the "protected bool m_Disposed" flag. 
        //     Do not re-declare it in derived classes.
        // B3. If a derived class owns any resources then you should follow rule A1,
        //     with the additional requirement that you should call base.Dispose().
        //     It is IMPERATIVE that you do this before setting m_Disposed = true;
        //     The way the virtual methods work means that the last N statements to
        //     execute will be "m_Disposed = true".
        // B4. If a derived class does NOT own any resources, then do nothing. Continue
        //     doing nothing all the way down the inheritance hierarchy until you
        //     encounter a case of rule B3.
        #endregion

        protected bool m_Disposed = false;

        /// <summary>
        /// CheckDisposed should be called at the beginning of every public method.
        /// [Optional: only do this in methods which need access to the owned resources to succeed.]
        /// </summary>
        /// <param name="method">The name of the method from which CheckDisposed is
        /// being called - can be got with MethodBase.GetCurrentMethod().Name.</param>
        protected virtual void CheckDisposed(string method) {
            if (m_Disposed)
                throw new ObjectDisposedException(null, "Detected in method: " + method);
        }

        /// <summary>
        /// SafeDisposeObject is a convenience routine which is used to called Dispose
        /// on an owned object. It checks that the object is not null, calls Dispose,
        /// then sets the object to null.
        /// </summary>
        /// <param name="managedObject">The object to dispose.</param>
        protected virtual void SafeDisposeObject(ref IDisposable managedObject) {
            if (managedObject != null) {
                managedObject.Dispose();
                managedObject = null;
            }
        }

        /// <summary>
        /// Any other cleanup which is not Dispose-related goes in NonDisposableCleanup().
        /// </summary>
        private void NonDisposableCleanup() {
        }

        /// <summary>
        /// public void Dispose() is the implementation of the IDispose interface.
        /// It calls the virtual Dispose() method, hence ensuring Disposes propagat
        /// down the inheritance hierarchy correctly.
        /// </summary>
        public void Dispose() {
            Dispose(true);
            // See link below to determine if a finalizer is required.
            // http://www.bluebytesoftware.com/blog/default,date,2005-12-29.aspx
            //GC.SuppressFinalize(this);    
        }

        /// <summary>
        /// protected virtual void Dispose(bool disposing) is the method that does the work.
        /// It will be overridden in any base classes.
        /// </summary>
        /// <param name="disposing">If true, the object is being explicitly disposed,
        /// e.g. via "using", not via a finalizer.</param>
        protected virtual void Dispose(bool disposing) {
            if (m_Disposed) return;

            if (disposing) {
                // Repeat for all owned managed objects that implement IDisposable.
                //SafeDisposeObject(ref m_OwnedObject);
            }

            NonDisposableCleanup();
            m_Disposed = true;
        }
        #endregion
    }

    // -------------------------------------------------------------------------------------

    class SubClassNoResources : BaseClass {
        public SubClassNoResources() : base() { }
    }

    // -------------------------------------------------------------------------------------

    class SubClassWithResources : SubClassNoResources {
        public SubClassWithResources() : base() { }

        #region IDisposable Implementation (for derived classes)
        // All you have to do is override the virtual Dispose method.
        // Note that the derived class does not inherit ( : IDisposable )
        // the IDisposable interface itself, the base class does it.
        private void NonDisposableCleanup() {
        }

        protected override void Dispose(bool disposing) {
            if (m_Disposed) return;

            if (disposing) {
                // Repeat for all owned managed objects that implement IDisposable.
                //SafeDisposeObject(ref m_OwnedObject);
            }

            NonDisposableCleanup();
            base.Dispose(disposing);
            m_Disposed = true;
        }
        #endregion
    }
}
