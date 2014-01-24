using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.DataInterfaces.Exceptions
{
    public class SavegameBuilderException : Exception
    {
        public SavegameBuilderException(string message)
            : base(message)
        {
        }
    }
}
