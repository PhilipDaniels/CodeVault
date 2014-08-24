using System;
using System.Collections.Generic;
using System.Text;

namespace AccessLevels {
    class Program {
        static void Main(string[] args) {
            // We can reference this OK, but not the PrivateNestedClass.
            // nor the ProtectedNestedClass.
            Parent.NestedClass nc = new Parent.NestedClass();
            
        }
    }

    /*
     * Definitions
     * ================================================================================================
     * public                       Access is not restricted.
     * protected                    Access is limited to the containing class or types derived from the containing class.
     *  (aka family)                       
     * internal                     Access is limited to the current assembly.
     *  (aka assembly)                      
     * protected internal           Access is limited to the current assembly **or** types derived from the containing class.
     *  (aka family or assembly)  
     * private                      Access is limited to the containing type.
     * 
     * "family and assembly" is not available in C#
     */

    // Top level classes (those at namespace level, not nested in others) can only be public or internal.
    // Public class is visible to anyone in or outside this assembly.
    // internal is the default.
    public class PublicClass { }
    internal class InternalClass { }

    // Won't compile, this is at namespace level so making it "private" makes no sense.
    //private class PrivateClass {}
     
    // Neither will these 2 things compile at name-space level. They don't make any sense.
    //protected class ProtectedClass { }
    //protected internal class ProtectedInternalClass { }

    // Protected and private are allowed on nested classes.
    //    Private means "not visible to my subclasses"
    //    Protected means "visible to my subclasses"
    //    Protected internal means "visible to my subclasses in this assembly only"

    class Parent {
        internal class NestedClass {
        }

        // This class cannot be referenced.
        private class PrivateNestedClass {
        }

        protected class ProtectedNestedClass {
        }
    }

    // To get hold of Parent.ProtectedNestedClass we must derive.
    class SecondParent : Parent {
        public SecondParent() {
            ProtectedNestedClass pnc = new ProtectedNestedClass();
        }
    }

}
