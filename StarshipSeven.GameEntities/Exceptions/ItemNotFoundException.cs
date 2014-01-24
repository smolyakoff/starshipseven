using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameEntities.Exceptions
{
    public class ItemNotFoundException : BaseEntityException
    {
        protected const string _msg = "Item {0} was not found in {1}";

        public ItemNotFoundException(object item, object container)
            :base(string.Format(_msg, item, container), null)
        {
        }
    }
}
