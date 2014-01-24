using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameEntities.Exceptions
{
    public class ItemDuplicateException : BaseEntityException
    {
        protected const string _msg = "Item {0} already exists in {1}";

        public ItemDuplicateException(object item, object container)
            :base(string.Format(_msg, item, container), null)
        {

        }

        public ItemDuplicateException(string message)
            : base(message, null)
        {
        }
    }
}
