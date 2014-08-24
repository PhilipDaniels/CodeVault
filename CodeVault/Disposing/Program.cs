using System;
using System.Collections.Generic;
using System.Text;

namespace Disposing {
    class Program {
        static void Main(string[] args) {
            BaseClass b1 = new BaseClass();
            b1.Dispose();

            SubClassNoResources b2 = new SubClassNoResources();
            b2.Dispose();

            SubClassWithResources b3 = new SubClassWithResources();
            b3.Dispose();
        }
    }
}
