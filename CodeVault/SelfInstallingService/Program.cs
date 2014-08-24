using System;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace SelfInstallingService
{
    /// <summary>
    /// This solution is a template for a self-installing and executing Windows service.
    /// 
    /// The resulting EXE can be run from the command line or passed flags (see the help)
    /// to install, uninstall, start and stop a Windows service.
    /// 
    /// TODO
    /// ====
    ///   1. Add your workload code to the Worker.DoWork() method.
    ///   2. Pull sleep time etc. from a config file rather than hardcoding.
    ///   3. You probably also want to add log4net to this program to get some decent logging.
    /// </summary>
    class Program
    {
        public static bool IsInteractive { get; set; }

        static void Main(string[] args)
        {
            IsInteractive = false;

            // The service can self [un]install.
            if (Environment.UserInteractive)
            {
                string parameter = String.Concat(args);
                switch (parameter)
                {
                    case "-i":
                        ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                        break;
                    case "-u":
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                        break;
                    case "-r":
                        RunInteractively();
                        break;
                    case "-stop":
                        StopTheService();
                        break;
                    case "-start":
                        StartTheService();
                        break;
                    default:
                        DisplayHelp();
                        break;
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new TheService() };
                ServiceBase.Run(ServicesToRun);
            }
        }

        static void StartTheService()
        {
            using (var controller = new ServiceController(Properties.Resources.ServiceName))
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(30 * 1000);
                controller.Start();
                controller.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
        }

        static void StopTheService()
        {
            using (var controller = new ServiceController(Properties.Resources.ServiceName))
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(30 * 1000);
                controller.Stop();
                controller.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
        }

        static void RunInteractively()
        {
            IsInteractive = true;
            var w = new Worker();
            w.DoWork();
        }

        static void DisplayHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Args (only one is allowed):");
            Console.WriteLine("    -i: Install as a Windows service.");
            Console.WriteLine("    -u: Uninstall the Windows service.");
            Console.WriteLine("    -start: Start the Windows service.");
            Console.WriteLine("    -stop: Stop the Windows service.");
            Console.WriteLine("    -r: Run interactively (CTRL-Q to stop).");
            Console.WriteLine();
            Console.WriteLine("There may be a considerable delay when stopping the service");
            Console.WriteLine("or interactive program. This is because the program may be");
            Console.WriteLine("sleeping (for a time controlled by the .config file) or in");
            Console.WriteLine("the middle of doing work, and the stop process waits for");
            Console.WriteLine("both of those things to conclude.");
            Console.WriteLine();
        }
    }
}
