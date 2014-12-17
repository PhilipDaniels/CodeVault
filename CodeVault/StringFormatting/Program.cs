using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace StringFormatting {
    class Program {
        static void Main(string[] args) {
            DoStrings();
        }

        private static void DoStrings() {
            string s1 = "foo";
            foreach (char c in s1)
                Console.WriteLine(c);

            Console.WriteLine(s1.GetHashCode());

            string[] strs = new string[] { "hello", "world", "Phil" };
            Console.WriteLine(String.Join(";", strs));


            // String formatting. {index[,alignment][:formatString]}
            // Note that what is valid for "format string" depends on the type being formatted.
            // eg D for ints means "decimal", but for DateTimes it means "long date format";
            Console.WriteLine("Hello {0, 10}", "world");
            Console.WriteLine("**{0}**", 22);
            Console.WriteLine("**{0,6}**", 22);
            Console.WriteLine("**{0,-6}**", 22);


            #region Standard Numeric Formats
            // D is decimal - no decimal places, just integers.
            Console.WriteLine("|{0:D}|", 420);

            // F is floating-point. The NumberFormatInfo static class controls how
            // many d.p.s you will get if you omit the precision specifier.
            Console.WriteLine("|{0:F}|", 500.3);
            Console.WriteLine("|{0:F5}|", 500.3);

            // N gives thousand separators.
            Console.WriteLine("|{0:N0}|", 4200000);
            #endregion

            #region NumberFormatInfo - doesn't work!
            // The NumberFormatInfo
            NumberFormatInfo nfi = NumberFormatInfo.CurrentInfo.Clone() as NumberFormatInfo;
            nfi.NumberGroupSeparator = "_";
            string s = 12131414.ToString("0:N0", nfi);
            Console.WriteLine(s);   // THIS DOESN'T WORK!!!
            #endregion

            #region Custom Numeric Formats
            // ms-help://MS.VSCC.v80/MS.MSDN.v80/MS.VisualStudio.v80.en/dv_fxfund/html/33f27256-722f-4296-a969-3a07dd4f2a02.htm

            Console.WriteLine("\n\nCustom formats");
            // '0' = [mandatory] zero placeholder, '#' = [optiona] digit placeholder
            // '.' = decimal point, ',' = thousands separator
            // '%' = percentage separator, ';' = section separator '\' = escape character


            Console.WriteLine("{0}", 12345);
            Console.WriteLine("{0000000000}", 12345);   // Error - just means param 0!!!
            Console.WriteLine("{0:000000000}", 12345);  // This is what I meant.
            Console.WriteLine("{0:000,000000}", 12345);  // Note you get them everywhere!
            Console.WriteLine("{0:000,000.000}", 12345);  // Note you get them everywhere!
            Console.WriteLine("{0:###,###.###}", 12345);  // Note you get them everywhere!

            for (int i = -5; i <= 5; i++) {
                // one section    = POS+NEG+ZERO
                // two sections   = POS+ZERO, NEG
                // three sections = POS, NEG, ZERO
                // 
                // Note that to get a - to appear you have to include it yourself.
                Console.WriteLine("{0:POS = 0;NEG = -0;ZER = 0}", i);
            }
            #endregion

            #region Standard DateTime Formats
            DateTime bday = new DateTime(1971, 12, 30);
            Console.WriteLine("{0:d}", bday);    // short date, same as now.ToShortDateString()
            Console.WriteLine("{0:D}", bday);    // long date
            Console.WriteLine("{0:F}", bday);    // "Full"
            Console.WriteLine("{0:s}", bday);    // ISO 8601
            #endregion

            #region Custom Date Formats
            // ms-help://MS.VSCC.v80/MS.MSDN.v80/MS.VisualStudio.v80.en/dv_fxfund/html/98b374e3-0cc2-4c78-ab44-efb671d71984.htm

            // yyyy = year as 4 digits
            // yy = year as 2 digits
            // MM = month as 2 digits
            // MMM = month as 3 character abbreviation
            // dd = day as 2 digits

            // hh = hour as 2 digits, range 1-12. HH = hour as 2 digits, range 0-23.
            // mm = minute as 2 digits
            // ss = seconds as 2 digits

            Console.WriteLine("{0:yyyy/MMM/dd hh:mm:ss}", bday);
            Console.WriteLine("{0:yyyy/MM/dd hh:mm:ss}", bday);

            // Note that to get a backslash to appear it takes a double escape - once to escape
            // the string that is in "", and then once to es    cape for the formatting engine.
            Console.WriteLine("{0:yyyy\\\\MM\\\\dd hh:mm:ss}", bday);

            // Or you can use an un-escaped string.
            Console.WriteLine(@"{0:yyyy\\MM\\dd hh:mm:ss}", bday);
            #endregion

            #region Formatting for a particular culture
            // Richter, p266.
            // Construct the relevant CultureInfo object and pass it into 
            // the object's ToString() method.
            decimal price = 124.4M;
            CultureInfo ci = new CultureInfo("en-US");
            string sp = price.ToString("C", ci);
            Console.WriteLine(sp);
            #endregion

            #region Implementing IFormattable on your objects
            FormattedObject fo = new FormattedObject(12);
            Console.WriteLine(fo.ToString("G", null)); // Will come out as a "G" number.
            Console.WriteLine(fo.ToString("G", new CultureInfo("en-US"))); // Will come out as "G" number.
            
            Console.WriteLine(fo.ToString("C", null)); // Will come out as UK currency.
            Console.WriteLine(fo.ToString("C", new CultureInfo("en-US"))); // Will come out as US currency.
            #endregion

            // For information on how to provide your own IFormatProvider:
            // ms-help://MS.VSCC.v80/MS.MSDN.v80/MS.NETDEVFX.v20.en/cpref2/html/T_System_IFormatProvider.htm
            // also Richter, p269.
        }
    }
}
