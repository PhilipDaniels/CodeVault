using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Delegates {
    class DelegatesWithLocalVar {

        public static void UseLocalVars(int numToDo) {
            int[] squares = new int[numToDo];
            AutoResetEvent done = new AutoResetEvent(false);

            // Do a bunch of tasks on other threads. Notice that the delegate refers to
            //   squares, done and numToDo
            // The CLR does this by creating a hidden class which captures these variables.

            for (int i = 0; i < squares.Length; i++) {
                ThreadPool.QueueUserWorkItem
                    (
                    delegate(object obj) {
                        int num = (int)obj;
                        squares[i] = num * num;
                        if (Interlocked.Decrement(ref numToDo) == 0)
                            done.Set();
                    },
                    i
                    );
            }

            // Wait for all other threads to finish.
            done.WaitOne();

            for (int i = 0; i < squares.Length; i++) {
                Console.WriteLine("squares[{0}] = {1}", i, squares[i]);
            }
        }

    }
}
