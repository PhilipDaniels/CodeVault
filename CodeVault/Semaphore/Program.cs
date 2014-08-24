using System;
using System.Collections.Generic;
using System.Text;
using TH = System.Threading;

namespace Semaphore {
    class Program {

        // A semaphore limits the number of threads that can
        // access a resource or pool of resources simultaneously.
        private static TH.Semaphore m_Pool;
        private static int m_Padding;

        static void Main(string[] args) {

            // Create a semaphore that can satisfy up to three
            // concurrent requests. Use an initial count of zero,
            // so that the entire semaphore count is initially
            // owned by the main program thread.
            m_Pool = new TH.Semaphore(0, 3);

            for (int i = 1; i <= 5; i++) {
                TH.Thread t = new TH.Thread(Worker);
                t.Start(i);
            }


            // Wait for half a second, to allow all the
            // threads to start and to block on the semaphore.
            TH.Thread.Sleep(500);

            // The main thread starts out holding the entire
            // semaphore count. Calling Release(3) brings the 
            // semaphore count back to its maximum value, and
            // allows the waiting threads to enter the semaphore,
            // up to three at a time.
            //
            Console.WriteLine("Main thread calls Release(3).");
            m_Pool.Release(3);
            Console.ReadLine();
            Console.WriteLine("Main thread exits.");
        }

        static void Worker(object num) {
            // Each worker thread begins by requesting the semaphore.
            Console.WriteLine("Thread {0} begins " + "and waits for the semaphore.", num);
            m_Pool.WaitOne();

            // OK, we're in.
            Console.WriteLine("Thread {0} enters the semaphore.", num);

            // A padding interval to make the output more orderly.
            int padding = TH.Interlocked.Add(ref m_Padding, 100);

            // The thread's "work" consists of sleeping for 
            // about a second. Each thread "works" a little 
            // longer, just to make the output more orderly.
            TH.Thread.Sleep(1000 + padding);

            Console.WriteLine("Thread {0} releases the semaphore.", num);
            Console.WriteLine("Thread {0} previous semaphore count: {1}", 
                num, m_Pool.Release());

        }
    }
}
