using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameEntities.Exceptions
{
    public class BaseEntityException : Exception
    {
        public BaseEntityException(string message, Exception innerException)
            :base(message, innerException)
        {
        }
    }
}
