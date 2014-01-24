using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameEntities.Events
{
    internal class FleetDeadEventArgs : EventArgs
    {
        internal FleetDeadEventArgs(Fleet deadFleet, Fleet killerFleet)
        {
            DeadFleet = deadFleet;
            KillerFleet = killerFleet;
        }

        internal Fleet DeadFleet { private set; get; }

        internal Fleet KillerFleet { private set; get; }
    }
}
