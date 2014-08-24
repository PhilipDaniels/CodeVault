using System;
using System.Threading;

namespace SelfInstallingService
{
    class Worker
    {
        // Volatile is used as hint to the compiler that this data 
        // member will be accessed by multiple threads.
        volatile bool ShouldStop;

        public void DoWork()
        {
            while (!ShouldStop)
            {
                try
                {
                    if (QuitKeyPressed())
                        break;

                    // TODO: Check queues for work here.

                    // TODO: Probably want to pull from the config file.
                    Thread.Sleep(5000);
                }
                catch (Exception ex)
                {
                    // log the error, exit or continue.
                }
            }
        }

        public void RequestStop()
        {
            ShouldStop = true;
        }

        /// <summary>
        /// The quit key is CTRL-Q.
        /// </summary>
        bool QuitKeyPressed()
        {
            if (Program.IsInteractive && Console.KeyAvailable)
            {
                var key = Console.ReadKey(false);
                if (((key.Modifiers & ConsoleModifiers.Control) != 0) && key.Key == ConsoleKey.Q)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
