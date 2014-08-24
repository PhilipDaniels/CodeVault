using System;
using System.Collections.Generic;
using System.Text;

namespace Encodings {
    class Program {
        static void Main(string[] args) {

            // Richter, p 274.
            // Encoding is typically needed when you need to send/read a string
            // to a file or network using System.IO.BinaryWriter or System.IO.StreamWriter

            string s = "Hello world";
            // (1) Various others exist. UTF8 is frequently used.
            // UTF7 is deprecated by the Unicode consortium.
            Encoding enc = Encoding.UTF32;               
            byte[] encodedBytes = enc.GetBytes(s);
            Console.WriteLine(BitConverter.ToString(encodedBytes));
            string decodedString = enc.GetString(encodedBytes);
            Console.WriteLine(decodedString);

            // (1) As well as the stock encodings, you can make a specific one.
            Encoding enc2 = Encoding.GetEncoding("SHIFT-JIS");
            
            // You will get an exception if you ask for something that doesn't exist.
            //Encoding enc3 = Encoding.GetEncoding("blah");

            // There are a lot of encodings available.
            EncodingInfo[] existingEncodings = Encoding.GetEncodings();
            Console.WriteLine("There are {0} encodings on this machine", existingEncodings.Length);
            foreach (EncodingInfo e in existingEncodings) {
                Console.WriteLine("{0}", e.Name);
            }


            // There are also encoders in System.Text. Constructors
            // allow control over the preamble etc.
            UTF7Encoding enc7 = new UTF7Encoding();
            UTF8Encoding enc8 = new UTF8Encoding();
            UTF32Encoding enc32 = new UTF32Encoding();
            UnicodeEncoding encUni = new UnicodeEncoding();
            ASCIIEncoding encASC = new ASCIIEncoding();         // Deprecated, use Encoding.ASCII instead.
            
            // Now you have an encoding, you use GetBytes to encode, GetString to decode.
            // ...
            
            // Base64 encoding/decoding. On the Convert class.
            // Base64 shrinks things down in size.
            byte[] someBytes = new byte[10];
            new Random().NextBytes(someBytes);
            Console.WriteLine(BitConverter.ToString(someBytes));
            string s64 = Convert.ToBase64String(someBytes);
            Console.WriteLine(s64);
            someBytes = Convert.FromBase64String(s64);
            Console.WriteLine(BitConverter.ToString(someBytes));

            // Encoding and decoding by chunks - useful when streaming data.

            // Don't use the encoder directly, do this to get special objects
            // which can cope with left-over bytes.
            //Decoder d8 = enc8.GetDecoder();
            //Encoder e8 = enc8.GetEncoder();

            // Example from ms-help://MS.VSCC.v80/MS.MSDN.v80/MS.NETDEVFX.v20.en/cpref12/html/T_System_Text_Decoder.htm
            // These bytes in UTF-8 correspond to 3 different Unicode
            // characters: space (U+0020), # (U+0023), and the biohazard
            // symbol (U+2623).  Note the biohazard symbol requires 3 bytes
            // in UTF-8 (hexadecimal e2, 98, a3).  Decoders store state across
            // multiple calls to GetChars, handling the case when one char
            // is in multiple byte arrays.
            byte[] bytes1 = { 0x20, 0x23, 0xe2 };
            byte[] bytes2 = { 0x98, 0xa3 };
            char[] chars = new char[3];

            Decoder d = Encoding.UTF8.GetDecoder();
            int charLen = d.GetChars(bytes1, 0, bytes1.Length, chars, 0);
            // The value of charLen should be 2 now.
            charLen += d.GetChars(bytes2, 0, bytes2.Length, chars, charLen);
            foreach (char c in chars)
                Console.Write("U+{0:X4}  ", (ushort)c);
        }
    }
}
