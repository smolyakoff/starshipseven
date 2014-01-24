using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameEntities.Exceptions
{
    public class TypeMismatchException : BaseEntityException
    {
        protected const string _msg = "The method expected type {0} instead of {1}";

        public TypeMismatchException(Type expectedObjectType, object actualObject)
            : base(string.Format(_msg, expectedObjectType, actualObject.GetType()), null)
        {

        }
    }
}
