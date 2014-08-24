using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace IsolatedStorage
{
    [IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed = IsolatedStorageContainment.ApplicationIsolationByMachine)]
    class Program
    {
        static void Main(string[] args)
        {
            BasicDemo();
        }

        private static void BasicDemo()
        {
            // There are 2 types of isolated storage available:
            //   Assembly/Machine   (ie application level data)
            //   Assembly/User      (ie user level data)

            // This works whether the assembly is a DLL (running in some other app) or an EXE.
            IsolatedStorageFile f1 = IsolatedStorageFile.GetMachineStoreForAssembly();
            IsolatedStorageFile f2 = IsolatedStorageFile.GetUserStoreForAssembly();     // Current user.

            // You would use this for ClickOnce applications (p128).
            //IsolatedStorageFile f3 = IsolatedStorageFile.GetMachineStoreForApplication();

            // This makes an app-level file within the isolated storage.
            IsolatedStorageFileStream appStream = new IsolatedStorageFileStream("AppSettings.txt", FileMode.OpenOrCreate, f1);
            IsolatedStorageFileStream userStream = new IsolatedStorageFileStream("UserSettings.txt", FileMode.OpenOrCreate, f2);

            // Then you use it just like any other stream...
            StreamWriter sw = new StreamWriter(appStream);

            // Existence.
            string[] files = f1.GetFileNames("MyFile.txt");
            if (files.Length == 0)
                Console.WriteLine("MyFile.txt does not exist");

            string[] dirs = f1.GetDirectoryNames("Blah*");
            if (dirs.Length == 0)
            {
                Console.WriteLine("No Blah* directories exist");
                // The directory will persist across program invocations.
                f1.CreateDirectory("Blah");
            }

            dirs = f1.GetDirectoryNames("Blah*");
            if (dirs.Length == 0)
            {
                Console.WriteLine("It still doesn't exist!!!! This is an error");
            }
            else
            {
                Console.WriteLine("It exists now");
            }
        }
    }
}
