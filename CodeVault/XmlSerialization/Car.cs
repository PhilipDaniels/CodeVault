using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

// Note the class must now be public.
// Only public data will be serialized. Private data will be ignored.
// Public properties will be serialized. If the property only has a GET then it will
// only be serialized, not deserialized.
// Note the class must also have a parameterless constructor.

namespace XmlSerialization {
    [XmlRoot("MyCar")]
    public class Car {
        [XmlAttribute]
        public string m_Model;
        private int m_Capacity;

        public int Capacity {
            get { return m_Capacity; }
            //set { m_Capacity = value; }
        }

        public Wheel[] m_Wheels;
        
        [XmlIgnore] public string m_Ignored;



        public Car()
            : this("", 0) {
        }

        public Car(string model, int capacity) {
            m_Model = model;
            m_Capacity = capacity;
            m_Wheels = new Wheel[4] 
                {
                    new Wheel(1000),
                    new Wheel(1000),
                    new Wheel(1000),
                    new Wheel(1000)
                };

            m_Ignored = "I am not serialized.";
        }

        // Events only work with BinaryFormatter and SoapFormatter....

        [OnSerializing]
        private void BeforeSer(StreamingContext context) {
            Console.WriteLine("Serialization is about to occur.");
        }

        [OnDeserializing]
        private void BeforeDeSer(StreamingContext context) {
            Console.WriteLine("DE-Serialization is about to occur.");
        }

        [OnSerialized]
        private void AfterSer(StreamingContext context) {
            Console.WriteLine("Serialization just occurred.");
        }

        [OnDeserialized]
        private void AfterDeSer(StreamingContext context) {
            Console.WriteLine("DE-Serialization just occurred.");
        }

    }
}
