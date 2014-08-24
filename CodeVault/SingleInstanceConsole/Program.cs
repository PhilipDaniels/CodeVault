using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading;

namespace SingleInstanceConsole {
    class Program {
        static void Main(string[] args) {
            // This works for a console app, but the console window does flash
            // onto the screen briefly.
            Assembly ass = Assembly.GetEntryAssembly();
            using (Mutex mutex = new Mutex(false, ass.FullName)) {
                bool owned = mutex.WaitOne(TimeSpan.Zero, false);
                if (owned) {
                    Console.ReadLine();
                }
            }
        }
    }
}
