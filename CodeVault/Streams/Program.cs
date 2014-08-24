using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

/*
 * Reading & Writing Using Raw Byte Streams
 * ========================================
 * Stream (abstract)
 *      IO.FileStream
 *      IO.BufferedStream
 *      IO.Compression.DeflateStream
 *      IO.Compression.GZipStream
 *      IO.MemoryStream
 *      + others, see Stream class documentation
 * Stream.Null (aka /dev/null)
 * IO.BinaryReader
 * IO.BinaryWriter
 * 
 * Reading & Writing using Encodings
 * =================================
 * These classes are the ones you want to use for text files.
 * 
 * TextReader (abstract)
 *      StringReader - read from a string
 *      StreamReader - read from an existing stream or filename
 * TextWriter (abstract)
 *      StringWriter - write to a string
 *      StreamWriter - write to an existing stream or filename
 * 
 */

namespace Streams {
    class Program {
        static void Main(string[] args) {
            TestStreamReaderWriter();
            TestStreamReaderWriter2();
            TestStreamWriterRandom();
            TestMemoryStream();
            TestBinaryFile();
            TestBufferedStream();
        }

        private static void TestBinaryFile() {
            const string file = @"C:\Temp\BinaryFile.txt";

            if (File.Exists(file))
                File.Delete(file);

            // Was attempting to make ASCII, Unicode etc, but I don't think it works.
            // If you want to create a Unicode file in binary, then you need to write
            // the pre-amble.
            //using (StreamWriter writer = new StreamWriter(file, false, Encoding.ASCII))
            //using (BinaryWriter bw = new BinaryWriter(writer.BaseStream)) {

            
            // Binary writers are constructed as wrappers around existing streams/reader-writers.
            // Note: only use the binary reader/writer for IO.
            using (FileStream fs = new FileStream(file, FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs)) {
                // Write the word "hello" in binary.
                bw.Write((byte)0x68);
                bw.Write((byte)0x65);
                bw.Write((byte)0x6C);
                bw.Write((byte)0x6C);
                bw.Write((byte)0x6F);
                bw.Write((byte)0xA3);
                // CRLF.
                bw.Write((byte)0x0D);
                bw.Write((byte)0x0A);

                bw.Write("Second string");
                bw.Write(42);
            }

            // Read it back as text.
            StreamReader rdr = new StreamReader(file);
            string s = rdr.ReadToEnd();
            Console.Write("Read from file: " + s);
            Console.WriteLine("Next line");
        }

        private static void TestStreamWriterRandom() {
            const string file = @"C:\Temp\file.txt";

            if (File.Exists(file))
                File.Delete(file);

            using (StreamWriter sw = new StreamWriter(file)) {
                sw.WriteLine("Line 1");
                sw.WriteLine("Line 2");
                sw.WriteLine("Line 3");

                FileStream fs = sw.BaseStream as FileStream;
                Debug.Assert(fs != null);

                // Without this flush() the pointer will not be reset to the beginning
                // of the file. Flushing fs does not work either.
                sw.Flush(); 
                fs.Seek(0, SeekOrigin.Begin);
                sw.WriteLine("Line A");
            }
        }

        private static void TestMemoryStream() {
            byte[] byteArray;
            ASCIIEncoding enc = new ASCIIEncoding();

            using (MemoryStream ms = new MemoryStream(1000)) {
                byteArray = enc.GetBytes("Line 1");
                ms.Write(byteArray, 0, byteArray.Length);
                byteArray = enc.GetBytes("Line 2");
                ms.Write(byteArray, 0, byteArray.Length);
                byteArray = enc.GetBytes("Line 3");
                ms.Write(byteArray, 0, byteArray.Length);

                // Now this works! Can seek OK in memory streams.
                // No flushing is required.
                ms.Seek(0, SeekOrigin.Begin);
                byteArray = enc.GetBytes("Line A");
                ms.Write(byteArray, 0, byteArray.Length);
            }
        }

        private static void TestStreamReaderWriter2() {
            const string file = @"C:\Temp\Afileio.txt";

            if (File.Exists(file))
                File.Delete(file);

            using (StreamWriter sw = new StreamWriter(file, false, Encoding.Unicode)) {
                sw.WriteLine("Line 1");
                sw.WriteLine("Line 2");
                sw.WriteLine("Line 3");
                sw.WriteLine(12);

                Stream bs = sw.BaseStream;
                if (bs.GetType() == typeof(FileStream))
                    Console.WriteLine("The stream writer 'sw' is backed by a FileStream, as you would expect.");

                if (bs.CanSeek)
                    Console.WriteLine("This FileStream can seek.");

                // If you don't flush flush then strange things happen in Notepad
                // because the encoding gets screwed up!
                sw.Flush();

                // This will write zero-bytes into the file. They look like
                // spaces in notepad, but check them in Emacs hexl-mode.
                bs.Seek(1000, SeekOrigin.Current);
                sw.WriteLine("The end");

                // This actually truncates the file!
                bs.Position = 0;
                sw.WriteLine("Actually the penultimate line written");

                // This lets us overwrite the beginning.
                bs.Seek(0, SeekOrigin.Begin);
                sw.WriteLine("aaa");
            }
        }

        private static void TestStreamReaderWriter() {
            const string file = @"C:\Temp\fileio.txt";
            const string file2 = @"C:\Temp\fileio2.txt";

            if (File.Exists(file))
                File.Delete(file);
            if (File.Exists(file2))
                File.Delete(file2);

            using (StreamWriter sw = new StreamWriter(file, false, Encoding.Unicode)) {
                sw.WriteLine("Line 1");
                sw.WriteLine("Line 2");
                sw.WriteLine("Line 3");
                sw.WriteLine(12);
            }

            if (File.Exists(file)) {
                using (StreamReader sr = new StreamReader(file, true)) {
                    string s = sr.ReadToEnd();
                    // This should be identical - check with ExamDiff. This proves that
                    // the encoding was detected correctly.
                    using (StreamWriter sw = new StreamWriter(file2, false, Encoding.Unicode)) {
                        sw.Write(s);
                    }
                }
            } else {
                throw new ApplicationException("The file should exist.");
            }
        }

        private static void TestBufferedStream() {
            // A BufferedStream is a type of stream that is wrapped around another stream
            // and provides buffering facilities, no matter the capabilities of the wrapped stream.
            const string file = @"C:\Temp\fileio.txt";

            if (File.Exists(file))
                File.Delete(file);

            using (FileStream fs = File.Create(file)) {
                using (BufferedStream buff = new BufferedStream(fs, 10000)) {
                    buff.WriteByte((byte)0x68);
                    buff.WriteByte((byte)0x65);
                    buff.WriteByte((byte)0x6C);
                    buff.WriteByte((byte)0x6C);
                    buff.WriteByte((byte)0x6F);
                    buff.WriteByte((byte)0xA3);
                    // CRLF.
                    buff.WriteByte((byte)0x0D);
                    buff.WriteByte((byte)0x0A);
                    buff.Flush();
                }
            }
        }
    }
}
