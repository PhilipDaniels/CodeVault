using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

// Richter, p218.
namespace Indexers {
    class BitArray {
        private byte[] m_Bytes;
        private int m_NumBits;

        public BitArray(int numBits) {
            if (numBits <= 0)
                throw new ArgumentOutOfRangeException("numBits", numBits.ToString(), "numBits must be > 0");

            m_NumBits = numBits;
            m_Bytes = new byte[(m_NumBits + 7) / 8];
        }

        // This is the syntax for an indexer. You can change the name of the generated
        // method by using the IndexerNameAttribute. However, this is irrelevant to C#.
        [IndexerName("Phil")]
        public bool this[int bitPos] {
            get {
                if (bitPos < 0 || bitPos >= m_NumBits)
                    throw new ArgumentOutOfRangeException("bitPos");

                return (m_Bytes[bitPos / 8] & (1 << (bitPos % 8))) != 0;
            }
            set {
                if (bitPos < 0 || bitPos >= m_NumBits)
                    throw new ArgumentOutOfRangeException("bitPos");

                if (value)
                    m_Bytes[bitPos / 8] = (byte)(m_Bytes[bitPos / 8] | (1 << (bitPos % 8)));
                else
                    m_Bytes[bitPos / 8] = (byte)(m_Bytes[bitPos / 8] & ~(1 << (bitPos % 8)));
            }
        }

        // You can overload indexers by parameter type.
        [IndexerName("Phil")]
        public bool this[string s] {
            get {
                return true;
            }
            set {
            }
        }
    }
}
