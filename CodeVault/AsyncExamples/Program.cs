using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // This will never print anything because the work is done on a background thread.
            // When the foreground thread terminates, all background threads are killed.
            //TaskCreation.DemonstrateTaskBeingKilledBeforeItCanComplete();

            // This method waits for the background task to complete.
            //TaskCreation.DemonstrateWaitingForTaskToComplete();

            // Shows that by default tasks run on the thread pool.
            //TaskCreation.DemonstrateDefaultThreadType();
            //TaskCreation.DemonstrateCustomThreadType();

            //TaskCreation.DemonstratePassingInputDataWithClosures();
            TaskCreation.DemonstrateReturningAValue();
        }
    }
}
