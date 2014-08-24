using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Attributes {

    // The meta data for this class (ie the attributions) will only be
    // emitted if one of these symbols is defined.
    [Conditional("TEST")][Conditional("VERIFY")]
    public sealed class MyConditionalAttribute : Attribute {
    }

    [MyConditional]     // Does nothing because neither TEST nor VERIFY are defined symbols.
    public class foo {
    }
}
