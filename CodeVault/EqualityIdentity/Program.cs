using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

// Richter p144-149 - how to implement Equals.
//
// 1. If the obj argument is null, return false because the current object identified by this
//    is obviously not null when the nonstatic Equals method is called.
// 2. If the this and obj arguments refer to objects of different types, return false. Obviously,
//    checking if a String object is equal to a FileStream object should result in a false result.
// 3. For each instance field defined by the type, compare the value in the this object with
//    the value in the obj object. If any fields are not equal, return false.
// 4. Call the base class's Equals method so it can compare any fields defined by it. If the base
//    class's Equals method returns false, return false; otherwise return true.


namespace EqualityIdentity {
    class Program {
        static void Main(string[] args) {

            // Rule 1:
            //   To check for identity, always use object.ReferenceEquals(obj1, obj2)

            // Rule 2 (when overriding Equals): 
            //   Equals is always value identity. Do not call the base object.Equals().
            //   Also override == and != to call Equals().
            //   override object.Equals() and Implement IEquatable<T>
            //       - object.Equals should call the type safe Equals()

            // Rule 3 (when your class can be ordered)
            //   implement Rule 2 above in terms of calls to IComparable<T>
            //   implement IComparable.CompareTo()
            //   implement IComparable<T>.CompareTo()
            //   implement <, <=, >=, and <.

            TestRule2();
            TestRule2Sub();
            TestClassVsSubClass();
            TestRule3();
        }

        static void TestRule2() {
            Rule2 a = new Rule2(12, "hello");
            Rule2 b = new Rule2(12, "hello");
            Rule2 c = new Rule2(13, "world");
            Rule2 d = null;

            Console.WriteLine("Equal objects");
            Console.WriteLine("Should print True: {0}", a.Equals(a));
            Console.WriteLine("Should print True: {0}", a.Equals(b));
            Console.WriteLine("Should print True: {0}", a == b);
            Console.WriteLine("Should print False: {0}", a != b);

            Console.WriteLine("\nUn-equal objects");
            Console.WriteLine("Should print False: {0}", a.Equals(c));
            Console.WriteLine("Should print False: {0}", a == c);
            Console.WriteLine("Should print True: {0}", a != c);

            Console.WriteLine("\nRHS is null");
            Console.WriteLine("Should print False: {0}", a.Equals(null));
            Console.WriteLine("Should print False: {0}", a == null);
            Console.WriteLine("Should print True: {0}", a != null);
            
            Console.WriteLine("\nLHS is null");
            Console.WriteLine("Should print False: {0}", Rule2.Equals(null, a));

            Console.WriteLine("\nBoth sides are null");
            Console.WriteLine("Should print True: {0}", Rule2.Equals(null, null));
        }

        static void TestRule2Sub() {
            Rule2Sub a = new Rule2Sub(12, "hello", 22);
            Rule2Sub b = new Rule2Sub(12, "hello", 22);
            Rule2Sub c = new Rule2Sub(13, "world", 98);
            Rule2Sub d = null;

            Console.WriteLine("\n\nEquality of subclassed objects\n");
            Console.WriteLine("Equal objects");
            Console.WriteLine("Should print True: {0}", a.Equals(a));
            Console.WriteLine("Should print True: {0}", a.Equals(b));
            Console.WriteLine("Should print True: {0}", a == b);
            Console.WriteLine("Should print False: {0}", a != b);

            Console.WriteLine("\nUn-equal objects");
            Console.WriteLine("Should print False: {0}", a.Equals(c));
            Console.WriteLine("Should print False: {0}", a == c);
            Console.WriteLine("Should print True: {0}", a != c);

            Console.WriteLine("\nRHS is null");
            Console.WriteLine("Should print False: {0}", a.Equals(null));
            Console.WriteLine("Should print False: {0}", a == null);
            Console.WriteLine("Should print True: {0}", a != null);

            Console.WriteLine("\nLHS is null");
            Console.WriteLine("Should print False: {0}", Rule2.Equals(null, a));

            Console.WriteLine("\nBoth sides are null");
            Console.WriteLine("Should print True: {0}", Rule2.Equals(null, null));

            
        }

        static void TestClassVsSubClass() {
            Console.WriteLine("\nClass vs derived class");
            
            Rule2 a = new Rule2(12, "hello");
            Rule2 b = new Rule2(12, "hello");
            Rule2 c = new Rule2(13, "world");
            Rule2 d = null;

            Rule2Sub asub = new Rule2Sub(12, "hello", 24);
            Rule2Sub bsub = new Rule2Sub(12, "hello", 24);
            Rule2Sub csub = new Rule2Sub(13, "world", 99);
            Rule2Sub dsub = null;

            Console.WriteLine("\ncompletely independent objects");
            Console.WriteLine("Should print False: {0}", a.Equals(asub));
            Console.WriteLine("Should print False: {0}", a.Equals(csub));
            Console.WriteLine("Should print False: {0}", a == asub);
            Console.WriteLine("Should print True: {0}", a != asub);

            Rule2 e = asub;
            // This selects Rule2.operator==() and returns true.
            // e is really a Rule2Sub, which is why GetType returns the same
            // as for asub.
            Console.WriteLine("Warning!!!! The compiler selects the best overload to call,");
            Console.WriteLine("which can result in slicing");
            Console.WriteLine("Should print True: {0}", e == asub);
            Console.WriteLine("Should print True: {0}", asub == e);

            // Get back to the original.
            // This will call Rule2Sub.operator==().
            Rule2Sub f = (Rule2Sub)e;
            Debug.Assert(f != null);
            Console.WriteLine("Should print True: {0}", f == asub);
            Console.WriteLine("Should print True: {0}", asub == f);
        }

        static void TestRule3() {
            Rule3 a = new Rule3(12, "hello");
            Rule3 b = new Rule3(12, "world");
            Rule3 c = new Rule3(12, "helloaaaa");
            Rule3 d = new Rule3(12, "aaaa");

            Rule3 e = new Rule3(5, "hello");
            Rule3 f = new Rule3(5, "world");
            Rule3 g = new Rule3(5, "helloaaaa");
            Rule3 h = new Rule3(5, "aaaa");

            Rule3 i = new Rule3(99, "hello");
            Rule3 j = new Rule3(99, "world");
            Rule3 k = new Rule3(99, "helloaaaa");
            Rule3 l = new Rule3(99, "aaaa");

            List<Rule3> list = new List<Rule3>();
            list.Add(a); list.Add(b); list.Add(c); list.Add(d);
            list.Add(e); list.Add(f); list.Add(g); list.Add(h);
            list.Add(i); list.Add(j); list.Add(k); list.Add(l);

            list.Sort();
            foreach (Rule3 elem in list)
                Console.WriteLine("{0,3} {1}", elem.TheInt, elem.TheString);

            Console.WriteLine("\nOperator <");
            Console.WriteLine("Should print False: {0}", a < a);
            Console.WriteLine("Should print True: {0}", a < b);
            Console.WriteLine("Should print True: {0}", a < c);
            Console.WriteLine("Should print False: {0}", a < null);
            Console.WriteLine("Should print True: {0}", null < a);
            Console.WriteLine("Should print False: {0}", (Rule3)null < (Rule3)null);

            Console.WriteLine("\nOperator <=");
            Console.WriteLine("Should print True: {0}", a <= a);
            Console.WriteLine("Should print True: {0}", a <= b);
            Console.WriteLine("Should print True: {0}", a <= c);
            Console.WriteLine("Should print False: {0}", a <= null);
            Console.WriteLine("Should print True: {0}", null <= a);
            Console.WriteLine("Should print True: {0}", (Rule3)null <= (Rule3)null);

            Console.WriteLine("\nOperator ==");
            Console.WriteLine("Should print True: {0}", a == a);
            Console.WriteLine("Should print False: {0}", a == b);
            Console.WriteLine("Should print False: {0}", a == c);
            Console.WriteLine("Should print False: {0}", a == null);
            Console.WriteLine("Should print False: {0}", null == a);
            Console.WriteLine("Should print True: {0}", (Rule3)null == (Rule3)null);

            Console.WriteLine("\nOperator !=");
            Console.WriteLine("Should print False: {0}", a != a);
            Console.WriteLine("Should print True: {0}", a != b);
            Console.WriteLine("Should print True: {0}", a != c);
            Console.WriteLine("Should print True: {0}", a != null);
            Console.WriteLine("Should print True: {0}", null != a);
            Console.WriteLine("Should print False: {0}", (Rule3)null != (Rule3)null);

            Console.WriteLine("\nOperator >=");
            Console.WriteLine("Should print True: {0}", a >= a);
            Console.WriteLine("Should print False: {0}", a >= b);
            Console.WriteLine("Should print False: {0}", a >= c);
            Console.WriteLine("Should print True: {0}", a >= null);
            Console.WriteLine("Should print False: {0}", null >= a);
            Console.WriteLine("Should print True: {0}", (Rule3)null >= (Rule3)null);

            Console.WriteLine("\nOperator >");
            Console.WriteLine("Should print False: {0}", a > a);
            Console.WriteLine("Should print False: {0}", a > b);
            Console.WriteLine("Should print False: {0}", a > c);
            Console.WriteLine("Should print True: {0}", a > null);
            Console.WriteLine("Should print False: {0}", null > a);
            Console.WriteLine("Should print False: {0}", (Rule3)null > (Rule3)null);

        }

    }
}
