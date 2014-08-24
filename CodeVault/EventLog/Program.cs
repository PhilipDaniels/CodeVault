using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace EventLog {
    class Program {
        static void Main(string[] args) {
            WriteToStandardLog();
            WriteToCustomLog();
            ReadSystemLog();
            MonitorEventLog();
        }

        private static void WriteToStandardLog() {
            // There are 3 standard event logs, Application, Security and System.
            const string source = "MySource";
            const string logName = "Application";

            // Note that creating an event source will throw if the source already exists.
            // Also, the source can only be associated with one log at a time.
            if (!System.Diagnostics.EventLog.SourceExists(source)) {
                System.Diagnostics.EventLog.CreateEventSource(source, logName);
            }

            using (System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog(logName)) {
                eventLog.Source = source;
                eventLog.WriteEntry("Hello world");
            }
        }

        private static void WriteToCustomLog() {
            const string source = "MySource2";  
            const string logName = "MyAppLog";

            if (!System.Diagnostics.EventLog.SourceExists(source)) {
                System.Diagnostics.EventLog.CreateEventSource(source, logName);
            }

            using (System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog(logName)) {
                eventLog.Source = source;
                eventLog.WriteEntry("Hello world in my dedicated log");
            }
        }

        private static void ReadSystemLog() {
            const string logName = "System";

            using (System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog(logName)) {
                EventLogEntryCollection entries = eventLog.Entries;
                foreach (EventLogEntry entry in entries) {
                    Console.WriteLine
                        (
                        "{0,12} {1:yyyy/MM/dd hh:mm:ss} {2,20} {3}",
                        entry.EntryType,
                        entry.TimeWritten,
                        entry.Source,
                        entry.Category
                        );
                }
            }
        }

        private static void MonitorEventLog() {
            // Monitor an EventLog by calling a delegate when an entry is written.
            const string source = "MySource2";
            const string logName = "MyAppLog";

            if (!System.Diagnostics.EventLog.SourceExists(source)) {
                System.Diagnostics.EventLog.CreateEventSource(source, logName);
            }

            using (System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog(logName)) {
                eventLog.Source = source;
                eventLog.EnableRaisingEvents = true;
                eventLog.EntryWritten += new EntryWrittenEventHandler(eventLog_EntryWritten);
                eventLog.WriteEntry("Hello world, watch an event popup in a few seconds");

                Console.WriteLine("Press \'q\' to quit.");
                while (Console.Read() != 'q') {
                    // Wait.
                }      
            }
        }

        private static void eventLog_EntryWritten(object sender, EntryWrittenEventArgs e) {
            Console.WriteLine("Got an entry written to the log: {0}", e.Entry.Message);
        }
    }
}
