using System;
using System.Collections.Generic;
using System.Text;

namespace Generics {
    // Richter, p384.
    class Issues {
        // How to set a generic variable to its default value if you don't
        // know whether it is a reference or struct type?
        public void SetToDefault<T>() {
            T temp = default(T);
        }

        // If we know it's a class we can do this...
        public void SetToDefault2<T>() where T : class {
            T temp = null;
        }

        // For structs we can call the default constructor.
        public void SetToDefault3<T>() where T : struct {
            T temp2 = new T();
        }


    }
}
