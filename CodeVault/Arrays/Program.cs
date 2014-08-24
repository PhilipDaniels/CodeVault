using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

// Collection interface hierarchy:
//      IEnumerable<T> : IEnumerable
//      ICollection<T> : IEnumerable<T>
//      IList<T> : ICollection<T>
//      IDictionary<T> : ICollection<T>

// Collection namespaces:
// In System.Collections: ArrayList, BitArray, HashTable, Queue, SortedList, Stack
//    + various others such as Comparer, CaseInsensitiveComparer and various abstract base classes
// In System.Collections.Specialized: HybridDictionary, ListDictionary, NameValueCollection, OrderedDictionary,
//      StringCollection, StringDictionary.
// In System.Collections.Generic: Dictionary (+), LinkedList, List, Queue, SortedDictionary, SortedList, Stack.


namespace Arrays {
    class Program {
        internal class RefType {
            public int m_Val;
            public RefType(int val) { m_Val = val; }
        }

        static void Main(string[] args) {
            // Richter, p 297.

            // Multi-dimensional arrays.
            // Don't think of this as "row, column", just "index1, index2, index3..."
            // and use them consistently.
            int[,] ia = new int[2, 4];
            int[, ,] ia2 = new int[2, 4, 8];

            ia[0, 0] = 10;
            ia[0, 1] = 20;

            Console.WriteLine("ia has {0} dimensions", ia.Rank);
            Console.WriteLine("The first one has indexes from {0} to {1} inclusive", ia.GetLowerBound(0), ia.GetUpperBound(0));
            Console.WriteLine("The second one has indexes from {0} to {1} inclusive", ia.GetLowerBound(1), ia.GetUpperBound(1));

            // Jagged arrays (arrays of arrays).
            int[][] ji = new int[3][];  // An array of length 3, each element being an integer array
            ji[0] = new int[10];
            ji[1] = new int[20];
            ji[2] = new int[30];

            Console.WriteLine(ia.ToString());       // System.Int32[,]
            int[,] icloned = (int[,])ia.Clone();    // Shallow copy, fine for value types.

            string[] s1 = new string[] { "hello", "world" };
            string[] s2 = (string[])s1.Clone();
            s1[0] = "mello";
            Console.WriteLine("s1[0] = {0}, but s2[0] = {1} - because strings are immutable!", s1[0], s2[0]);
            
            // If we do that again with a reference type, things are different.
            RefType[] r1 = new RefType[] { new RefType(10), new RefType(20) };
            RefType[] r2 = (RefType[])r1.Clone();
            r1[0].m_Val = 30;
            Console.WriteLine("r1[0] = {0}, and r2[0] = {1} - because the Clone() was shallow", r1[0].m_Val, r2[0].m_Val);

            // All arrays derive from System.Array. This class has many interesting static methods, 
            // including generic ones. It's a bit like C++ algorithm.
            //  Array.AsReadOnly
            //  Array.BinarySearch, <T>
            //  Array.Clear(..)
            //  Array.Exists
            int[] rand = new int[30];
            Random rgen = new Random();
            for (int i = 0; i < rand.Length; i++)
                rand[i] = rgen.Next(20);

            // Now filter the list using a named method and then an anonymous delegate.
            int[] lt = Array.FindAll(rand, LessThan);  
            int[] lt2 = Array.FindAll(rand, delegate(int i) { return i < 3; });

            Array.ForEach(lt, delegate(int i) { Console.WriteLine("lt = {0}", i); });
            Array.ForEach(lt2, delegate(int i) { Console.WriteLine("lt2 = {0}", i); });

            // Copying. It will throw an exception if the destination isn't big enough.
            // Copy can be used to effect type conversions, see Richter p298.
            int[] l3 = new int[3];
            Array.Copy(lt, l3, 3);

            // All arrays implement these 3 interfaces. Note that some methods won't
            // work, eg Add, because arrays are fixed size.
            Process((IEnumerable<int>)l3);
            Process((ICollection<int>)l3);
            Process((IList<int>)l3);

            // If your array is of a derived reference type, then the generic interfaces
            // will also exist for each of the base types in your inheritance hierarchy.
            // IList<base>, IList<higherbase>, IList<highestbase> etc.
        }

        internal static bool LessThan(int i) {
            return i < 10;
        }

        internal static void Process(IEnumerable<int> a) {
            foreach (int i in a)
                Console.WriteLine("IEnumerable, = {0}", i);
        }

        internal static void Process(ICollection<int> a) {
            Console.WriteLine("ICollection, there are {0} elements in the collection", a.Count);
        }

        internal static void Process(IList<int> a) {
        }
    
    }
}
