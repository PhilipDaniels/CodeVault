using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiscUtils.Framework
{
    /*
    /// <summary>
    /// Base class to contain common code for associating things of
    /// arbitrary type with the log4net context.
    /// </summary>
    /// <typeparam name="TThing">Type of thing to be associated.</typeparam>
    public abstract class AssocThing<TThing> : IDisposable
    {
        string Key;

        public AssocThing(string key, TThing thing)
        {
            Key = key;
            log4net.ThreadContext.Properties[Key] = thing;
        }

        public void Dispose()
        {
            log4net.ThreadContext.Properties[Key] = null;
        }

        internal static TThing Get(string key)
        {
            object thing = log4net.ThreadContext.Properties[key];
            if (thing == null)
            {
                return default(TThing);
            }
            else
            {
                return (TThing)Convert.ChangeType(thing, typeof(TThing));
            }
        }

        internal static void Set(string key, TThing thing)
        {
            log4net.ThreadContext.Properties[key] = thing;
        }
    }

    /// <summary>
    /// Class for associating arbitrary ints with the log.
    /// </summary>
    public sealed class AssocInt : AssocThing<int?>
    {
        public const string ASSOC_INT = "AssocInt";

        public AssocInt(int? value)
            : base(ASSOC_INT, value)
        {
        }

        public static int? Value
        {
            get { return Get(ASSOC_INT); }
            set { Set(ASSOC_INT, value); }
        }
    }


    /// <summary>
    /// Class for associating arbitrary strings with the log.
    /// </summary>
    public sealed class AssocString : AssocThing<string>
    {
        public const string ASSOC_STRING = "AssocString";

        public AssocString(string value)
            : base(ASSOC_STRING, value)
        {
        }

        public static string Value
        {
            get { return Get(ASSOC_STRING); }
            set { Set(ASSOC_STRING, value); }
        }
    }
    */
}
