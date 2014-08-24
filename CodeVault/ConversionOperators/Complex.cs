using System;
using System.Collections.Generic;
using System.Text;

namespace ConversionOperators {
    class Complex {
        private double m_re, m_im;

        public Complex(double Real, double Imag) {
            m_re = Real;
            m_im = Imag;
        }

        // Conversion constructors.
        public Complex(int Real) {
            m_re = Real;
            m_im = 0;
        }

        public Complex(double Real) {
            m_re = Real;
            m_im = 0;
        }

        // Conversion methods.
        public int ToInt32() {
            return (int)m_re;
        }

        public double ToDouble() {
            return m_re;
        }


        // Conversion operators - to my type.
        public static implicit operator Complex(int Real) {
            return new Complex(Real);
        }

        public static implicit operator Complex(double Real) {
            return new Complex(Real);
        }

        // Conversion operators - from my type.
        public static implicit operator int(Complex rhs) {
            return rhs.ToInt32();
        }

        public static implicit operator double(Complex rhs) {
            return rhs.ToDouble();
        }

    }
}
