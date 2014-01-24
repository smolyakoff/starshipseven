using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameEntities.Exceptions
{
    public class GameLogicException : BaseEntityException
    {
        public GameLogicException(string message)
            : base(message, null)
        {
        }
    }
}
