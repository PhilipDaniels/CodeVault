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
        Cursor originalCursor;
        bool isDisposed = false;

        public CursorKeeper(Cursor newCursor)
        {
            originalCursor = Cursor.Current;
            Cursor.Current = newCursor;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    Cursor.Current = originalCursor;
                }
            }

            isDisposed = true;
        }

        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
