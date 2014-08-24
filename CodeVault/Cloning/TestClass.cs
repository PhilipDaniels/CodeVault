using System;
using System.Collections.Generic;
using System.Text;

// Based on code at:
// http://www.windojitsu.com/blog/copyctorvsicloneable.html

// use the "icl" snippet to insert this stuff.

namespace Cloning {
    class TestBase : ICloneable {
        private int m_BaseState;

        // Normal constructor (not really relevant to this discussion).
        public TestBase(int state) {
            m_BaseState = state;
        }

        // Copy constructor. Makes implementing cloning trivial.
        public TestBase(TestBase rhs) {
            m_BaseState = rhs.m_BaseState;
        }

        // General clone method required by the ICloneable interface.
        object ICloneable.Clone() {
            return Clone();
        }

        // Type-safe clone method (not required by the interface, but very nice to have).
        // Note that neither method is virtual.
        public TestBase Clone() {
            return new TestBase(this);
        }
    }

    class TestDerived : TestBase, ICloneable {
        private double m_DerivedState;

        public TestDerived(int intState, double doubleState) : base(intState) {
            m_DerivedState = doubleState;
        }

        public TestDerived(TestDerived rhs) : base(rhs) {
            m_DerivedState = rhs.m_DerivedState;
        }

        object ICloneable.Clone() {
            return Clone();
        }

        public new TestDerived Clone() { 
            return new TestDerived(this);
        }
    }
}
