using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace чм4к2.Exceptions
{
    public class StepException : Exception
    {
        public StepException(string message) : base(message) { }
    }
}
