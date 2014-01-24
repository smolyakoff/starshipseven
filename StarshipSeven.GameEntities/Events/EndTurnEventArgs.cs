using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;

namespace StarshipSeven.GameEntities.Events
{
    public class EndTurnEventArgs : EventArgs
    {
        public EndTurnEventArgs(IEnumerable<AttackFleet> launchedFleets)
        {
            LaunchedFleets = new List<AttackFleet>(launchedFleets);
        }

        public IEnumerable<AttackFleet> LaunchedFleets { private set; get; }
    }
}
