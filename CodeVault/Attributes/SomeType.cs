using System;
using System.Collections.Generic;
using System.Text;

// Richter, p390.
// "m" indicates that the attribute prefix is mandatory. For others, the
// compiler will infer them.

// [assembly:m Att]
// [module:m Att]



namespace Attributes {
    //[type: Att]
    class SomeType
        <
        //[typevar: Att]
        T>
    {
        //[field: Att]
        public int foo;

        // [return:m Att]
        // [method: Att]
        public int MyMethod
            (
            // [param: Att]
            int x
            )
        {
            return 1;
        }

        // [property: Att]
        public string MyProp {
            // [method: Att]
            get {
                return "foo";
            }
        }

        // [event: Att]     applied to the event
        // [field: Att]     applied to the compiler-generated field
        // [method: Att]    applied to the compiler-generated add & remove methods.
        public event EventHandler SomeEvent;
    }
}
