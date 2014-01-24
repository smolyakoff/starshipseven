using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.DataInterfaces.Exceptions
{
    public class DataConnectionException : DataException
    {
        public DataConnectionException(string message, Exception innerException)
            :base(message, innerException)
        {

        }
    }
}
