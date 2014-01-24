using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.DataInterfaces.Exceptions
{
    public class DataException : Exception
    {
        public DataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
