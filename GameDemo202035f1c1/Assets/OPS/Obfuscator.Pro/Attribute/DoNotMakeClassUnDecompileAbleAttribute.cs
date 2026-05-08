using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPS.Obfuscator.Attribute
{
    /// <summary>
    /// Add this to an Class, to disallow making it unreadable by dissassembler!
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DoNotMakeClassUnDecompileAbleAttribute : System.Attribute
    {
    }
}
