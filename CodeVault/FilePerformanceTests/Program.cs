using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePerformanceTests
{
    class Program
    {
        // In my tests you need at least 40 files before the parallel version is faster.
        const int NUM_FILES = 100;

        static void Main(string[] args)
        {
            //CreateTestFiles();
            TimeMethod("ReadFilesInSerial", () => ReadFilesInSerial());         // 96 msec to read 15Mb from 100 files.
            TimeMethod("ReadFilesInParallel", () => ReadFilesInParallel());     // 65 msec to read 15Mb from 100 files.
        }

        static void ReadFilesInSerial()
        {
            for (int i = 0; i < NUM_FILES; i++)
            {
                string filename = GetTestFileName(i);
                string contents = File.ReadAllText(filename);
                //Console.WriteLine("Read {0} chars from {1}", contents.Length, filename);
            }
        }

        static void ReadFilesInParallel()
        {
            Parallel.For(0, NUM_FILES, (i) =>
                {
                    string filename = GetTestFileName(i);
                    string contents = File.ReadAllText(filename);
                    //Console.WriteLine("Read {0} chars from {1}", contents.Length, filename);
                });
        }

        static void TimeMethod(string name, Action action)
        {
            var sw = new Stopwatch();
            sw.Start();
            action();
            sw.Stop();
            Console.WriteLine("{0} took {1} msec.", name, sw.ElapsedMilliseconds);
        }


        static void CreateTestFiles()
        {
            for (int i = 0; i < NUM_FILES; i++)
            {
                CreateTestFile(GetTestFileName(i));
            }
        }

        static string GetTestFileName(int i)
        {
            return String.Format(@"C:\temp\perftests\testfile{0}.txt", i);
        }

        static void CreateTestFile(string filename)
        {
            var r = new Random();
            int numCharsPerLine = r.Next(20, 100);
            int numLines = r.Next(30, 5000);
            //int numCharsPerLine = 10;
            //int numLines = 3;

            using (var fs = new FileStream(filename, FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                for (int lineNum = 0; lineNum < numLines; lineNum++)
                {
                    for (int charNum = 0; charNum < numCharsPerLine; charNum++)
                    {
                        char c = (char)r.Next(32, 93);
                        sw.Write(c);
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}
