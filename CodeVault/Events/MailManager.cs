using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Richter, p227.
 * 
 * Defining a type that raises events
 * ==================================
 * 1. Define a type (EventArgs) to hold any information you need to pass in the event.
 *    (You can pass EventArgs.Empty if you have nothing to say).
 *
 * 2. Define the delegate type that clients will have to use to response to
 *    the event. This can be skipped if one of the existing handlers, such as
 *    EventHandler<>, can be used.
 * 
 * 3. Define the event member. This will usually be public so that clients can access it.
 *    (There is not much point to internal events).
 * 
 * 4. Define a method responsible for raising the event.
 * 
 * 5. Define a method that actually *calls* the method defined in step 3 in 
 *    response to appropriate criteria.
 */

namespace Events
{
    class MailManager
    {
        // Step 1.
        internal class NewMailEventArgs : EventArgs
        {
            private readonly string m_From;
            private readonly string m_To;
            private readonly string m_Subject;

            public NewMailEventArgs(string from, string to, string subject)
            {
                m_From = from;
                m_To = to;
                m_Subject = subject;
            }

            public string From { get { return m_From; } }
            public string To { get { return m_To; } }
            public string Subject { get { return m_Subject; } }
        }

        // Step 3.
        // Define a public event...
        // called NewMail...
        //.. with a delegate type of EventHandler<NewMailEventArgs>
        //
        // This generic has a definition of
        //    public delegate void EventHandler<TEventArgs>
        //       (object sender, TEventArgs e) where TEventArgs : EventArgs
        // so your event handlers must look like this
        //    void MethodName(object sender, NewMailEventArgs e);
        public event EventHandler<NewMailEventArgs> NewMail;

        // Step 4.
        // Define a method to raise the event.
        protected virtual void OnNewMail(NewMailEventArgs e)
        {
            // Save in temp for thread safety (other threads may be
            // adding or removing event handlers).
            EventHandler<NewMailEventArgs> temp = NewMail;

            if (temp != null)
                temp(this, e);
        }


        // Step 5.
        // Raise the event at appropriate times.
        public void SimulateNewMail(string from, string to, string subject)
        {
            NewMailEventArgs e = new NewMailEventArgs(from, to, subject);
            OnNewMail(e);
        }

    }
}
