using System;
using System.Collections.Generic;
using System.Text;

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


namespace EqualityIdentity {
    class Rule3 : IEquatable<Rule3>, IComparable<Rule3>, IComparable {
        private int m_Int;
        private string m_String;

        public Rule3(int TheInt, string TheString) {
            m_Int = TheInt;
            m_String = TheString;
        }

        public int TheInt { get { return m_Int; } }
        public string TheString { get { return m_String; } }

        #region IEquatable, IComparable<T> and IComparable Implementation
        public int CompareTo(object obj) {
            return CompareTo((Rule3)obj);
        }
        
        public int CompareTo(Rule3 other) {
            // other == null   --> return 1 (because this must exist)
            // this < other    --> return negative number
            // this == other   --> return 0
            // this > other    --> return positive number

            if ((object)other == null)
                return 1;

            // Place the type into a partial order based upon the ordering of its fields.
            int comp;
            comp = m_Int.CompareTo(other.m_Int);
            if (comp != 0) return comp;
            comp = m_String.CompareTo(other.m_String);
            return comp;

            // TODO
            // If this type is derived from object just return the final comp,
            // otherwise return base.CompareTo(other).
        }

        public override bool Equals(object obj) {
            return Equals((Rule3)obj);
        }

        public bool Equals(Rule3 other) {
            if (object.ReferenceEquals(other, null))
                return false;
            else {
                // First check for an exact type match.
                if (!object.ReferenceEquals(GetType(), other.GetType()))
                    return false;

                int comp = CompareTo(other);
                return comp == 0;
            }
        }

        public static bool operator ==(Rule3 lhs, Rule3 rhs) {
            if (object.ReferenceEquals(lhs, null))
                return object.ReferenceEquals(rhs, null);
            else
                return lhs.Equals(rhs);
        }

        public static bool operator !=(Rule3 lhs, Rule3 rhs) {
            return !(lhs == rhs);
        }

        public static bool operator <(Rule3 lhs, Rule3 rhs) {
            if (object.ReferenceEquals(lhs, null))
                return !object.ReferenceEquals(rhs, null);
            else {
                int comp = lhs.CompareTo(rhs);
                return comp < 0;
            }
        }

        public static bool operator <=(Rule3 lhs, Rule3 rhs) {
            if (object.ReferenceEquals(lhs, null))
                return true;
            else {
                int comp = lhs.CompareTo(rhs);
                return comp <= 0;
            }
        }

        public static bool operator >(Rule3 lhs, Rule3 rhs) {
            if (object.ReferenceEquals(lhs, null))
                return false;
            else {
                int comp = lhs.CompareTo(rhs);
                return comp > 0;
            }
        }

        public static bool operator >=(Rule3 lhs, Rule3 rhs) {
            if (object.ReferenceEquals(lhs, null))
                return object.ReferenceEquals(rhs, null);
            else {
                int comp = lhs.CompareTo(rhs);
                return comp >= 0;
            }
        }
        #endregion
    }
}
