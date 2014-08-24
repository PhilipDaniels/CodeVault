using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace XmlSerialization {
    class Program {
        static void Main(string[] args) {
            bool sout = true;

            if (sout) {
                Car c = new Car("Ford", 1600);
                FileStream fs = new FileStream(@"C:\Temp\car.txt", FileMode.Create);
                XmlSerializer bf = new XmlSerializer(typeof(Car));
                bf.Serialize(fs, c);
            } else {
                FileStream fs = new FileStream(@"C:\Temp\car.txt", FileMode.Open);
                XmlSerializer bf = new XmlSerializer(typeof(Car));
                Car c = (Car)bf.Deserialize(fs);
            }


        }
    }
}
