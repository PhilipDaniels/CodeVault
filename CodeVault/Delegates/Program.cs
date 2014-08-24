using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Delegates {

    // Richter, p331.
    class Program {

        // A delegate is a type-safe function pointer.
        // This declares a delegate **type** which is a class derived from MulticastDelegate.
        // It has 3 private variables _target, _methodPtr, _invocationList
        // and 3 methods: Invoke, BeginInvoke, EndInvoke.

        // In reflector, you can see a class called "Feedback".
        internal delegate void Feedback(int value);

        static void Main(string[] args) {
            StaticDelegateDemo();
            InstanceDelegateDemo();
            ChainDelegateDemo1(new Program());

            // Syntax shortcut 1: No need to explicitly construct a delegate object.
            // QueueUserWorkItem requires a "WaitCallback" delegate.
            // The compiler will make one for us.
            ThreadPool.QueueUserWorkItem(SomeAsyncTask, 5);

            // Syntax shortcut 2: No need to define a callback method
            // The "delegate" keyword can also be used to construct an anonymous method.
            // If you examine the exe in ILDASM you can see it.
            ThreadPool.QueueUserWorkItem(delegate(object obj) { Console.WriteLine(obj); }, 5);

            // Syntax shortcut 3: No need to specify callback params if you don't use them.
            // This is especially handy for event handlers.
            ThreadPool.QueueUserWorkItem
                (
                delegate { Console.WriteLine("I'm not using the object parameter that WaitCallback normally takes"); }, 
                null
                );

            // There is no button1 in this project but this is how it would work...
            //button1.Click += delegate { MessageBox.Show("I don't use 'object sender' or 'EventArgs e'"); };

            DelegatesWithLocalVar.UseLocalVars(5);
        }

        private static void SomeAsyncTask(object obj) {
            Console.WriteLine(obj);
        }

        private static void StaticDelegateDemo() {
            Console.WriteLine("----- Static Delegate Demo -----");
            Counter(1, 3, null);
            Counter(1, 3, new Feedback(Program.FeedbackToConsole));
            Counter(1, 3, new Feedback(FeedbackToMsgBox));
            Console.WriteLine();
        }

        private static void InstanceDelegateDemo() {
            Console.WriteLine("----- Instance Delegate Demo -----");
            Program p = new Program();
            // p will be passed as the "this" pointer. p will be held inside,
            // the delegate class, so it won't be Garbage collected until the
            // delegate is GC'ed.
            Counter(1, 3, new Feedback(p.FeedbackToFile));
            Console.WriteLine();
        }

        private static void ChainDelegateDemo1(Program p) {
            Console.WriteLine("----- ChainDelegateDemo1 Delegate Demo -----");
            Feedback fb1 = new Feedback(FeedbackToConsole);
            Feedback fb2 = new Feedback(FeedbackToMsgBox);
            Feedback fb3 = new Feedback(p.FeedbackToFile);

            // Chaining results in the _invocationList being set to an array
            // with each reference being to one of the delegates in the list.
            Feedback fbChain = null;
            fbChain = (Feedback)Delegate.Combine(fbChain, fb1);
            fbChain = (Feedback)Delegate.Combine(fbChain, fb2);
            fbChain = (Feedback)Delegate.Combine(fbChain, fb3);
            Counter(1, 2, fbChain);

            // Also see Delegate.Remove, +=, -= which are calls to Combine/Remove.

            // To control delegate invocation yourself, do this:
            Delegate[] ds = fbChain.GetInvocationList();
            foreach (Feedback f in ds) {
            }

            Console.WriteLine();
        }

        // This method takes a delegate as it's last parameter.
        private static void Counter(int from, int to, Feedback fb) {
            for (int i = from; i <= to; i++)
                if (fb != null)
                    fb(i);
        }

        private static void FeedbackToConsole(int i) {
            Console.WriteLine("Item = " + i.ToString());
        }

        private static void FeedbackToMsgBox(int i) {
            MessageBox.Show("Item = " + i.ToString());
        }

        // This is an instance method, so it has an implicit "this" pointer.
        // See InstanceDelegateDemo()
        private void FeedbackToFile(int i) {
            StreamWriter sw = new StreamWriter("Status", true);
            sw.WriteLine("Item = " + i.ToString());
            sw.Close();
        }
    
    }
}
