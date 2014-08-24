using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace StringFormatting {
    class FormattedObject : IFormattable {
        private int m_MyData;

        public FormattedObject(int data) {
            m_MyData = data;
        }

        #region IFormattable Members
        // The non-parameterized ToString() should be implemented simply in terms of IFormattable.ToString();
        public override string ToString() {
            return ToString(null, null);
        }

        // You can provide convenience overloads as you think fit.
        public string ToString(string format) {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            // If no format, default to general. This routine gets to determine
            // what the meanings of these codes are. Throw System.FormatException
            // if an unknown code is passed in.
            if (String.IsNullOrEmpty(format))
                format = "G";

            // Case 1: No formatProvider is needed. Just go ahead and format the
            // object using the format string.

            // Case 2: A format provider is required.
            
            // If no format provider is specified default to the current thread's info.
            // The FCL only implements this interface on 3 types:
            //     CultureInfo, NumberFormatInfo, DateTimeFormatInfo.
            // If the caller has provided a format provider, query it to get the format.
            NumberFormatInfo nfi;
            if (formatProvider == null) {
                nfi = NumberFormatInfo.CurrentInfo;
            } else {
                nfi = formatProvider.GetFormat(typeof(NumberFormatInfo)) as NumberFormatInfo;
            }

            // Use the format provider we were told to.
            return String.Format(nfi, "{0:" + format + "}", m_MyData);
            //return m_MyData.ToString(nfi);

            //// If you don't need a format provider, then just go ahead and format 
            //// using the format string, else call formatProvider.ToString().
            //if (format.Equals("G", StringComparison.OrdinalIgnoreCase))
            //    return "foo";
            //return "bar";
        }
        #endregion
    }
}
