using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameInterfaces.Events
{
    public class ReinforcementsArrivedEventArgs : EventArgs
    {
        public int Ships { private set; get; }

        public IPlanet Planet { private set; get; }

        public ReinforcementsArrivedEventArgs(IPlanet planet, int ships)
        {
            Planet = planet;
            Ships = ships;
        }
    }
}
