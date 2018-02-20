using System;
using System.Collections.Generic;
using System.Text;

namespace SgtinAppCore
{
    public class SgtinException : Exception
    {
        public SgtinException(string message) : base(message)
        {
        }

        public SgtinException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }  
}
