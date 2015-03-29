using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OldMiscUtils
{
    public class CursorKeeper : IDisposable
    {
        Cursor OriginalCursor;
        Control[] ControlsToDisable;
        bool IsDisposed;

        public CursorKeeper()
            : this(Cursors.WaitCursor)
        {
        }

        public CursorKeeper(params Control[] controlsToDisable)
            : this(Cursors.WaitCursor, controlsToDisable)
        {
        }

        public CursorKeeper(Cursor newCursor)
            : this(newCursor, null)
        {
        }

        public CursorKeeper(Cursor newCursor, params Control[] controlsToDisable)
        {
            OriginalCursor = Cursor.Current;
            Cursor.Current = newCursor;
            ControlsToDisable = controlsToDisable;
            EnableControls(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    Cursor.Current = OriginalCursor;
                    EnableControls(true);
                    ControlsToDisable = null;
                }
            }

            IsDisposed = true;
        }

        /// <summary>
        /// Undoes the keep operation, restoring the original cursor
        /// and re-enabling any controls that were disabled.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void EnableControls(bool enable)
        {
            if (ControlsToDisable != null)
            {
                foreach (var ctrl in ControlsToDisable)
                    ctrl.Enabled = enable;
            }
        }
    }
}
