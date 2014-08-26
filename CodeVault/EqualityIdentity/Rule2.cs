using System;
using System.Collections.Generic;
using System.Text;

// Rule 2 (when overriding Equals): 
//   Equals is always value identity. Do not call the base object.Equals().
//   Also override == and != to call Equals().
//   override object.Equals() and Implement IEquatable<T>
//       - object.Equals should call the type safe Equals()

namespace EqualityIdentity {
    class Rule2 : IEquatable<Rule2> {
        private int m_Int;
        private string m_String;

        public Rule2(int TheInt, string TheString) {
            m_Int = TheInt;
            m_String = TheString;
        }

        #region IEquatable<Rule2> Members
        public override bool Equals(object obj) {
            return Equals((Rule2)obj);
        }

        public bool Equals(Rule2 other) {
            if ((object)other == null)
                return false;

            // First check for an exact type match.
            if (!object.ReferenceEquals(GetType(), other.GetType()))
                return false;

            // If this type does not derive from object then add a call to base.Equals().
            return m_Int == other.m_Int &&
                m_String == other.m_String;
        }

        public static bool operator ==(Rule2 lhs, Rule2 rhs) {
            //Naive recursive loop: return lhs == null ? rhs == null : lhs.Equals(rhs);
            return (object)lhs == null ? (object)rhs == null : lhs.Equals(rhs);
        }

        public static bool operator !=(Rule2 lhs, Rule2 rhs) {
            //Naive recursive loop: return lhs == null ? rhs != null : !lhs.Equals(rhs);
            //return (object)lhs == null ? (object)rhs != null : !lhs.Equals(rhs);
            return !(lhs == rhs);
        }
        #endregion

        public override int GetHashCode()
        {
            var h = new SpookilySharp.SpookyHash();
            h.Update(m_Int);
            h.Update(m_String);
            return h.Final().GetHashCode();
        }
    }

    // OK, this is a derived class.
    class Rule2Sub : Rule2, IEquatable<Rule2Sub> {
        private int m_SecondInt;

        public Rule2Sub(int TheInt, string TheString, int SecondInt)
            : base(TheInt, TheString)
        {
            m_SecondInt = SecondInt;
        }

        #region IEquatable<Rule2Sub> Members
        public override bool Equals(object obj) {
            return Equals((Rule2Sub)obj);
        }

        public bool Equals(Rule2Sub other) {
            if ((object)other == null)
                return false;

            // First check for an exact type match.
            if (!object.ReferenceEquals(GetType(), other.GetType()))
                return false;

            // If this type does not derive from object then add a call to base.Equals().
            return m_SecondInt == other.m_SecondInt &&
                base.Equals(other);
        }

        public static bool operator ==(Rule2Sub lhs, Rule2Sub rhs) {
            //Naive recursive loop: return lhs == null ? rhs == null : lhs.Equals(rhs);
            return (object)lhs == null ? (object)rhs == null : lhs.Equals(rhs);
        }

        public static bool operator !=(Rule2Sub lhs, Rule2Sub rhs) {
            //Naive recursive loop: return lhs == null ? rhs != null : !lhs.Equals(rhs);
            //return (object)lhs == null ? (object)rhs != null : !lhs.Equals(rhs);
            return !(lhs == rhs);
        }
        #endregion

        public override int GetHashCode()
        {
            var h = new SpookilySharp.SpookyHash();
            h.Update(m_SecondInt);
            h.Update(base.GetHashCode());
            return h.Final().GetHashCode();
        }
    }
}
