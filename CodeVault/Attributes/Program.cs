using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;

[assembly: CLSCompliant(true)]

namespace Attributes {

    [Serializable]
    [DefaultMember("Main")]
    class Program {
        [Conditional("Debug")]
        [Conditional("Release")]
        public void DoSomething() { }

        // An attribute is a class. When the compiler sees this, it generates a BlahAttribute3
        // class and serializes its instance out into the methoddef metadata for the Main method.
        [BlahAttribute3(12, Y = "Hello")]
        [STAThread, CLSCompliant(true)]
        public static void Main(string[] args) {

            // To use attributes, you use reflection to determine if they are defined
            // on your class/method etc.

            // We need to get a "member info" for this method. MemberInfo is actually an abstract
            // base class, but we can use the MethodBase class derived from it...
            MethodBase mb = MethodBase.GetCurrentMethod();
            bool isDef = Attribute.IsDefined(mb, typeof(BlahAttribute2));

            PerfTest();

            

            Type progType = typeof(Program);
            ShowAttributes(progType);

            MemberInfo[] members = progType.FindMembers
                (
                MemberTypes.All,
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static,
                Type.FilterName,
                "*");

            foreach (MemberInfo member in members)
                ShowAttributes(member);

            // Other things:
            // Attribute matching: Richter, p401.
            // Attribute reflection, Richter p403.
        }

        /// <summary>
        /// Test the speed of the MethodBase.GetCurrentMethod() method.
        /// The loop really does run 1 million times, you can check with the ILDASM tool.
        /// </summary>
        private static void PerfTest() {
            MethodBase mb;
            for (int i = 0; i < 1000 * 1000; i++) {
                mb = MethodBase.GetCurrentMethod();
            }
        }

        private static void DummyCall(MemberInfo attributeTarget) { }

        private static void ShowAttributes(MemberInfo attributeTarget) {
            Attribute[] atts = Attribute.GetCustomAttributes(attributeTarget);

            Console.WriteLine("Attributes applied to {0}: {1}",
                attributeTarget.Name, (atts.Length == 0) ? "None" : String.Empty);

            foreach (Attribute att in atts) {
                Console.WriteLine("    {0}", att.GetType().ToString());

                if (att is DefaultMemberAttribute)
                    Console.WriteLine("    MemberName={0}", ((DefaultMemberAttribute)att).MemberName);
                if (att is ConditionalAttribute)
                    Console.WriteLine("    ConditionString={0}", ((ConditionalAttribute)att).ConditionString);
                if (att is CLSCompliantAttribute)
                    Console.WriteLine("    IsCompliant={0}", ((CLSCompliantAttribute)att).IsCompliant);
            }

            Console.WriteLine();
        }
    }
}
