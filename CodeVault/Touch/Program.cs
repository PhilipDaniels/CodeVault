using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Touch
{
    class Program
    {
        private static DateTime TimeToSet { get; set; }
        private static bool Silent { get; set; }
        private static int NumFilesProcessed { get; set; }
        private static int NumDirectoriesProcessed { get; set; }

        static void Main(string[] args)
        {
            TimeToSet = DateTime.Now;
            NumFilesProcessed = 0;
            NumDirectoriesProcessed = 0;
            Silent = false;

            if (args.Length == 0)
            {
                Console.WriteLine(TimeToSet.ToString());
                return;
            }

            if (args[0].Contains("?") || args[0].ToUpperInvariant().Contains("HELP"))
            {
                Console.WriteLine
                    (
                    "Touch usage: " + Environment.NewLine + 
                    "Touch -> no arguments returns current date time" + Environment.NewLine + 
                    "Touch path -> sets all files (recursively) under path to current time" + Environment.NewLine + 
                    "Touch path time [-s] -> sets all files in path to 'time', where time is any time format parseable. -s option means silent." + Environment.NewLine + 
                    "Example: touch c:\\temp\\pd \"2008-Feb-03 12:45\" -s"
                    );
                return;
            }

            if (args.Length > 1)
            {
                TimeToSet = DateTime.Parse(args[1]);
            }

            if (args.Length > 2)
            {
                if (args[2].ToUpperInvariant().Contains("S"))
                    Silent = true;
            }

            string fileSpec = args[0];

            if (!Directory.Exists(fileSpec))
            {
                Msg(fileSpec + " does not exist.");
            }

            DirectoryInfo di = new DirectoryInfo(fileSpec);
            ProcessDirectory(di);

            Console.WriteLine();
            Msg(String.Format("Changed times on {0} directories and {1} files.", NumDirectoriesProcessed, NumFilesProcessed));
        }

        private static void Msg(string msg)
        {
            Console.WriteLine("Touch: " + msg);
        }

        private static void ProcessDirectory(DirectoryInfo dir)
        {
            dir.CreationTime = TimeToSet;
            dir.LastAccessTime = TimeToSet;
            dir.LastWriteTime = TimeToSet;
            NumDirectoriesProcessed++;
            if (!Silent)
                Console.WriteLine(dir.FullName + ": set File Times to " + TimeToSet.ToString());

            FileInfo[] fi = dir.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in fi)
            {
                file.CreationTime = TimeToSet;
                file.LastAccessTime = TimeToSet;
                file.LastWriteTime = TimeToSet;
                NumFilesProcessed++;

                if (!Silent)
                    Console.WriteLine(file.FullName + ": set File Times to " + TimeToSet.ToString());
            }

            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                ProcessDirectory(subDir);
            }
        }
    }
}
