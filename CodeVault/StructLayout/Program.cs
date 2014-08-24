using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace StructLayout {
    class Program {
        static void Main(string[] args) {
        }
    }

    // This is the default for value types. It defines compatibility with C.
    // LayoutKind.Auto is the default for classes.
    // LayoutKind.Explicit allows you to create unions.
    [StructLayout(LayoutKind.Sequential)]
    struct MyStruct {
        byte m_Byte;
        int m_int;
    }
}
