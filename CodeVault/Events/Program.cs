using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * IMPORTANT
 * =========
 * Richter, p233.
 * If an object is registered with an event then it will NOT be garbage
 * collected. Therefore if the object implements IDisposable it should unregister
 * all event handlers. Unregistering multiple (more than necessary) times is OK.
 * 
 * IMPORTANT
 * =========
 * Richter, p234.
 * 
 * Thread safety and locking issues. C# locks the whole object which can lead
 * to deadlocks, and any code can lock/unlock the object. This can be worked around
 * by doing your own locking inside special "add" and "remove" methods. See 
 * MailManager2.
 * 
 * 
 * IMPORTANT
 * =========
 * Richter, p237.
 * Techniques presented in this example use one delegate field per event.
 * What if you have 80 events? You do not want to have 80 fields because most
 * of these events will not be used. Solution: maintain a dictionary of
 * delegates. See the "TypeWithLotsOfEvents" example from his book.
 */

namespace Events
{
    class Program
    {
        static void Main(string[] args)
        {
            MailManager mm = new MailManager();
            Fax f = new Fax(mm);
            Fax f2 = new Fax(mm);
            mm.SimulateNewMail("Phil", "Roger", "Test email subject");

            f2.Unregister();
            f2.Unregister();
        }
    }
}
