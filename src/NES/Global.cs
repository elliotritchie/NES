using System;
using System.Collections.Generic;

namespace NES
{
    internal class Global
    {
        private static IEnumerable<Type> _typesToScan = new List<Type>();
        internal static IEnumerable<Type> TypesToScan
        {
            get { return _typesToScan; }
            set { _typesToScan = value; }
        }
    }
}