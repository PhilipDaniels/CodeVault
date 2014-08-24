using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace SingleInstanceWinForms {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // This technique works well for WinForms, only one form is ever displayed.
            Assembly ass = Assembly.GetEntryAssembly();
            using (Mutex mutex = new Mutex(false, ass.FullName)) {
                bool owned = mutex.WaitOne(TimeSpan.Zero, false);
                if (owned) {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
            }
        }
    }
}