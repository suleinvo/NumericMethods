﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace чм4к2.Exceptions
{
    public class InputException : Exception
    {
        public InputException(string message) : base(message) { }
    }
}
