using System;
using System.Collections.Generic;
using System.Text;

namespace ConversionOperators {
    class Program {
        static void Main(string[] args) {
            // Conversion constructor:
            //      public YourType(SomeOtherType rhs) {}
            //
            // Conversion method:
            //      public int ToInt32() { }
            //
            // Conversion operator - to your type, these allow mixed expressions:
            //      public static implicit operator YourType(SomeOtherType rhs) {
            //          return new YourType(rhs);
            //      }
            //
            // Conversion operator - from your type, these allow mixed expressions:
            //      public static implicit operator int(YourType) {
            //          return YourType.ToInt32();
            //      }
            //
            // implicit = no cast required. Only use for non-narrowing conversions.
            // explicit = cast required


            // Call conversion constructors
            Complex a = new Complex(2);
            Complex b = new Complex(3.5);
            Complex c = new Complex(3.5, Math.PI);

            // Call conversion methods.
            Console.WriteLine(b.ToInt32().ToString());
            Console.WriteLine(b.ToDouble().ToString());

            // Call conversion operators - from complex.
            int v = c;
            double w = c;

            // Call conversion operators - to complex.
            Complex x = 3;
            Complex y = 5.7;


            // Guidelines
            // Always provide ToString().
            // Implicit conversion operators must be non-narrowing.
        }
    }
}
