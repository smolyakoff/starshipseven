using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameEntities.Exceptions
{
    public class CheatingException : BaseEntityException
    {
        public Player Cheater { private set; get; }

        public CheatingException(Player cheater)
            :base(string.Format("Cheating detected by {0}", cheater), null)
        {
            Cheater = cheater;
        }

        public CheatingException()
            : base("Cheating detected!", null)
        {

        }
    }
}
