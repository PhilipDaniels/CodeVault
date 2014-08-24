using System;
using System.Collections.Generic;
using System.Text;

namespace Cloning {
    class Program {
        static void Main(string[] args) {
            TestDerived t1 = new TestDerived(12, 43.3);
            TestDerived t2 = t1.Clone();
        }
    }
}
