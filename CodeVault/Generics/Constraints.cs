using System;
using System.Collections.Generic;
using System.Text;

namespace Generics {
    class Constraints {
        // Richter, p377.
        // A generic method with a constraint. If you comment out the constraint it
        // won't compile because T is not guaranteed to contain CompareTo().
        public static T Min<T>(T obj1, T obj2) where T : IComparable<T>
        {
            return (obj1.CompareTo(obj2) < 0) ? obj1 : obj2;
        }

        // Constraints cannot be used to create new overloads. The following
        // won't compile because it is considered a duplicate of the above.
        //public static T Min<T>(T obj1, T obj2) where T : IEquatable<T> { }
        //public static T Min<T>(T obj1, T obj2) where T : IComparable<T>, ICloneable { }

        // Overloading of generic is by type-arity only, even if you don't use all the parameters.
        // Note that FXCop doesn't spot this unused parameter.
        public virtual T Min<T, U>(T obj1, T obj2)
            where T : IComparable<T>
            where U : ICloneable
        {
            return (obj1.CompareTo(obj2) < 0) ? obj1 : obj2;
        }
    }

    class Derived : Constraints {
        // When you override a virtual generic you don't (can't) specify the constraints.
        public override T Min<T, U>(T obj1, T obj2) {
            return base.Min<T, U>(obj1, obj2);
        }
    }
}
