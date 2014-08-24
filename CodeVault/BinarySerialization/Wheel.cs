using System;
using System.Collections.Generic;
using System.Text;

namespace BinarySerialization {
    [Serializable]
    class Wheel {
        private int m_Pressure;

        public Wheel(int pressure) {
            m_Pressure = pressure;
        }
    }
}
