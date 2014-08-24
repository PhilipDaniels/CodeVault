using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Generics {
    // Richter, p 380.
    class ConstraintExamples {
        // Any reference type.
        public void Foo1<T>() where T : class { }

        // No constraint --> T is "object" (not very useful).
        public void Foo2<T>() { } 

        // Any value type (except nullable types) - there is no way to specify nullability.
        public void Foo3<T>() where T : struct { }

        // Any Stream or subclass of Stream (slicing occurs).
        public void Foo6<T>() where T : Stream { }

        // Secondary constraints.
        // T must be compatible with U (T is a U or any subclass of U).
        public void Foo7<T, U>() where T : U { }

        // Constructor constraints.
        // T must be a class that has a parameterless constructor. It is not possible
        // to specify any parameters to new() in the constraint. 
        // For value types, this is not legal and not needed because all value types
        // already are guaranteed to have a parameterless constructor.
        public T Factory<T>() where T : new() { return new T(); }
    }
}
