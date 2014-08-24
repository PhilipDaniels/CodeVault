using System;
using System.Collections.Generic;
using System.Text;

namespace Exiting {
    class Program {
        static void Main(string[] args) {

            // This will gracefully shut down the Windows Process when called
            // from anywhere in your application. All finalizers are run before
            // the program finishes, ensuring all unmanaged resources are released.
            System.Environment.Exit(0);

            // For WinForms, see Application.Exit() or Application.ExitThread().
            // They raise the Closing() event on any open forms, notifying them
            // and giving them a chance to cancel.
        }
    }
}
