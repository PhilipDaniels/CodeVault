using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading;

/*
 * Hierarchy of reflection types, Richter, p566.
 * Note that the abstract classes provide useful static methods.
 * 
 * System.Object
 *      System.Reflection.MemberInfo (abstract)
 *          System.Type (main type you will need for reflection, provides GetFields() etc)
 *          System.Reflection.FieldInfo
 *          System.Reflection.MethodBase (abstract)
 *              System.Reflection.ConstructorInfo
 *              System.Reflection.MethodInfo
 *          System.Reflection.PropertyInfo
 *          System.Reflection.EventInfo
 * 
 * Richter, p570. Walking the reflection object model.
 * From An          You Can Navigate To
 * =======          ===================
 * AppDomain        Assembly
 * Assembly         Module, Type
 * Module           Type
 * Type             FieldInfo, ConstructorInfo, MethodInfo, PropertyInfo, EventInfo
 *                  (and vice versa, back to the Type via ReflectedType)
 * *Info            ParameterInfo
 */

namespace Reflection {
    class Program {
        static void Main(string[] args) {

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies) {
                WriteLine(0, "Assembly: {0}", assembly);

                foreach (Type type in assembly.GetExportedTypes()) {
                    WriteLine(1, "Type: {0}", type);

                    const BindingFlags flags = BindingFlags.DeclaredOnly |
                        BindingFlags.NonPublic |
                        BindingFlags.Public |
                        BindingFlags.Instance |
                        BindingFlags.Static;

                    foreach (MemberInfo memberInfo in type.GetMembers(flags)) {
                        string typeName = String.Empty;
                        if (memberInfo is Type) typeName = "(Nested) Type";
                        else if (memberInfo is FieldInfo) typeName = "FieldInfo";
                        else if (memberInfo is MethodInfo) typeName = "MethodInfo";
                        else if (memberInfo is ConstructorInfo) typeName = "ConstructorInfo";
                        else if (memberInfo is PropertyInfo) typeName = "PropertyInfo";
                        else if (memberInfo is EventInfo) typeName = "EventInfo";

                        WriteLine(2, "{0}: {1}", typeName, memberInfo);
                    }
                }
            }
        }

        private static void WriteLine(int indent, string format, params object[] args) {
            Console.WriteLine(new string(' ', 3 * indent) + format, args);
        }
    }
}
