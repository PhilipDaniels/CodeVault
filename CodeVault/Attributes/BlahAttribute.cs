using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes {
    // Can be applied to any target.
    public class BlahAttribute : Attribute {
        public BlahAttribute() { }
    }


    // Can be applied to any enums only.
    // There are 2 named parameters, Inherited (def. true) and AllowMultiple (def. false).
    [AttributeUsage(AttributeTargets.Enum, Inherited = false)]
    public class BlahAttribute2 : Attribute {
        public BlahAttribute2() { }
    }


    // This shows how to create an attribute with some mandatory parameters
    // (passed to the constructor) and some optional ones (properties).
    // Note that a Type can be used as a parameter (clients must use typeof() operator).
    public class BlahAttribute3 : Attribute {
        private int m_X;
        private string m_Y;

        public BlahAttribute3(int x) {
            m_X = x;
        }

        public int X {
            get { return m_X; }
            set { m_X = value; }
        }

        public string Y {
            get { return m_Y; }
            set { m_Y = value; }
        }
    }
}
