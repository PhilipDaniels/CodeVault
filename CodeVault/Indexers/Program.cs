using System;
using System.Collections.Generic;
using System.Text;

namespace Indexers {
    class Program {
        static void Main(string[] args) {
            BitArray a = new BitArray(12);
            for (int i = 0; i < 12; i++)
                a[i] = (i % 2 == 0);

            for (int i = 0; i < 12; i++)
                Console.WriteLine("Bit {0} is {1}", i, a[i] ? "On" : "Off");
        }
    }
}
