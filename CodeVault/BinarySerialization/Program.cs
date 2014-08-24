using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BinarySerialization {
    class Program {
        static void Main(string[] args) {
            bool sout = true;

            if (sout) {
                Car c = new Car("Ford", 1600);
                FileStream fs = new FileStream(@"C:\Temp\car.txt", FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, c);
            } else {
                FileStream fs = new FileStream(@"C:\Temp\car.txt", FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                Car c = (Car)bf.Deserialize(fs);
            }
        }
    }
}
