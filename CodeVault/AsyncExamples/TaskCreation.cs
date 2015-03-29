using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncExamples
{
    static class TaskCreation
    {
        public static void DemonstrateTaskBeingKilledBeforeItCanComplete()
        {
            Task t = Task.Factory.StartNew(Speak);
        }

        public static void DemonstrateWaitingForTaskToComplete()
        {
            Task t = Task.Factory.StartNew(Speak);
            t.Wait();
        }

        public static void DemonstrateDefaultThreadType()
        {
            Task t = Task.Factory.StartNew(WhatTypeOfThreadAmI);
            t.Wait();
        }

        public static void DemonstrateCustomThreadType()
        {
            Task t = Task.Factory.StartNew(WhatTypeOfThreadAmI, TaskCreationOptions.LongRunning);
            t.Wait();
        }

        public static void DemonstratePassingInputDataWithClosures()
        {
            /* This is how to pass data into tasks. This is a closure which gets re-written by the compiler into a class.
             * i.e. the compiler transforms a method needing parameters into one that does not. You can examine the
             * code in a decompiler to see it.
             * 
             * sealed class theClosure {
             *   public DataImporter importer;
             *   public string dir;
             *   public void closureMethod() {
             *     importer.Import(dir);
             *   }
             * }
             * 
             * and the code below gets transformed to:
             * var c = new theClosure();
             * c.importer = new DataImporter();
             * c.dir = @"C:\temp";
             * Task.Factory.StartNew(importer.ClosureMethod);
             */

            var importer = new DataImporter();
            string dir = @"C:\temp";
            Task t = Task.Factory.StartNew(() => importer.Import(dir));
            t.Wait();
        }

        public static void DemonstrateReturningAValue()
        {
            // Task is the type for a task that does not return a value. Such tasks take an Action<...> delegate.
            // Task<TResult> is the type for a task that returns a value. Such tasks take a Func<TResult...> delegate.
            Task<int> t = Task.Factory.StartNew<int>(() => { Thread.Sleep(2000); return 42; });
            int x = t.Result;

            // This variant can be useful when you need to return a task but you already have
            // a result available (perhaps from synchronous code). The created task is already
            // in a completed state, and its Result property can be read without blocking.
            var t2 = Task.FromResult("42");
        }

        static void Speak()
        {
            Console.WriteLine("Hello world");
        }

        static void WhatTypeOfThreadAmI()
        {
            Console.WriteLine("I am a {0} thread.", Thread.CurrentThread.IsThreadPoolThread ? "Thread Pool" : "Custom");
        }

        static void Speak(string message)
        {
            Console.WriteLine(message);
        }
    }
}
