using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.DataInterfaces.Exceptions
{
    public class ItemNotFoundException : DataException
    {
        private const string _msg = "Item {0} was not found in {1}";
        public ItemNotFoundException(object obj, object container, Exception innerException)
            :base(string.Format(_msg, obj, container), innerException)
        {

        }
    }
}
