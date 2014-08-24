using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

// BinarySerialization works fine with private data (members and the class itself
// are both private in this case). Essentially you need
// to make no changes other than to mark your class (and all contained classes)
// with the [Serializable] attribute and optionally respond to the events.

namespace BinarySerialization {
    [Serializable]
    class Car {
        private string m_Model;
        private int m_Capacity;
        private Wheel[] m_Wheels;
        [NonSerialized] private string m_Ignored;

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
