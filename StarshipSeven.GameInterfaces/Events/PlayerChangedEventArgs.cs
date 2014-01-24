using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameInterfaces.Events
{
    public class PlayerChangedEventArgs : EventArgs
    {
        public PlayerChangedEventArgs(IPlayer player)
        {
            Player = player;
        }

        public IPlayer Player { private set; get; }
    }
}
