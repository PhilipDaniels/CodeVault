using System.ServiceProcess;
using System.Threading;

namespace SelfInstallingService
{
    /// <summary>
    /// This class is a simple shim deriving from ServiceBase so that
    /// it can be installed.
    /// </summary>
    partial class TheService : ServiceBase
    {
        Worker WorkerObject;
        Thread WorkerThread;

        public TheService()
        {
            InitializeComponent();
            ServiceName = Properties.Resources.ServiceName;
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            if (WorkerObject != null)
                return;

            // Start a new background thread to run the guts of the service.
            WorkerObject = new Worker();
            WorkerThread = new Thread(new ThreadStart(WorkerObject.DoWork));
            WorkerThread.Start();
        }

        protected override void OnStop()
        {
            base.OnStop();

            if (WorkerObject != null)
            {
                WorkerObject.RequestStop();
                WorkerThread.Join();
            }
        }
    }
}
