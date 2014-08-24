using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Richter, p232.
 * 
 * Defining a type that listens for events
 * =======================================
 * 1. Define a sink (a method matching the event delegate signature) in this class.
 *    IntelliSense will help you will it in if you press TAB after the +=.
 * 
 * 2. Subscribe the sink using +=.
 */

namespace Events
{
    class Fax
    {
        private MailManager m_MailManager;

        public Fax(MailManager mm)
        {
            // This is the long-handed way to add an event handler. IntelliSense adds
            // this when you press tab...and tab again inserts the method stub in this class.
            //mm.NewMail += new EventHandler<MailManager.NewMailEventArgs>(mm_NewMail);
            m_MailManager = mm;
            mm.NewMail += FaxMsg;
        }

        // Step 1.
        void FaxMsg(object sender, MailManager.NewMailEventArgs e)
        {
            Console.WriteLine("Faxing message");
            Console.WriteLine("    From={0}, To={1}, Subject={2}", e.From, e.To, e.Subject);
        }

        public void Unregister()
        {
            m_MailManager.NewMail -= FaxMsg;
        }
    }
}
