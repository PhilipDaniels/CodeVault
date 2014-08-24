using System;
using System.Collections.Generic;
using System.Text;

namespace Operators {
    class Program {
        static void Main(string[] args) {
            // All operator overloads are public static methods.
            // Unary operators take 1 argument, binary operators take 2.

            //     public static Complex operator !(Complex lhs)
            //     public static Complex operator +(Complex lhs, Complex rhs)
            
            // Unary operators that can be overloaded
            // Symbol   CLS Compliant Method Name
            // ------   -------------------------
            // +        Plus            These all return the class type
            // -        Minus
            // !        Not
            // ~        OnesComplement
            // ++       Increment
            // --       Decrement

            // Binary operators that can be overloaded
            // Symbol   CLS Compliant Method Name
            // ------   -------------------------
            // +        Add             These all return the class type
            // -        Subtract
            // *        Multiply
            // /        Divide
            // %        Mod

            // &        BitwiseAnd      These only apply to integral or bitset types
            // |        BitwiseOr       They all return the class type
            // ^        Xor
            // <<       LeftShift
            // >>       RightShift

            // ==       Equals          Use the eqc snippet for these operators
            // !=       Compare         They all return bool.
            // <        Compare
            // >        Compare
            // <=       Compare
            // >=       Compare

        }

        class Foo {
            public static Foo operator ++(Foo lhs) {
                return lhs;
            }
        }

    }
}
