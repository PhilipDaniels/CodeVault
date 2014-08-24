using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    class MailManager2
    {
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

        // private field to use for thread synchronization locking.
        private readonly object m_EventLock = new object();

        // Head of the delegate list.
        private EventHandler<NewMailEventArgs> m_NewMail;

        public event EventHandler<NewMailEventArgs> NewMail
        {
            add
            {
                lock (m_EventLock) { m_NewMail += value; }
            }
            remove
            {
                lock (m_EventLock) { m_NewMail -= value; }
            }
        }

        protected virtual void OnNewMail(NewMailEventArgs e)
        {
            EventHandler<NewMailEventArgs> temp = m_NewMail;
            if (temp != null)
                temp(this, e);
        }

        public void SimulateNewMail(string from, string to, string subject)
        {
            NewMailEventArgs e = new NewMailEventArgs(from, to, subject);
            OnNewMail(e);
        }
    }
}
