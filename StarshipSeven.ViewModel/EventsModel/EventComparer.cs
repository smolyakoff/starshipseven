using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;

namespace StarshipSeven.ViewModel.EventsModel
{
    public class EventComparer : IComparer<IGameEvent>
    {

        public EventComparer(IPlayer player)
        {
            CurrentPlayer = player;
        }

        public IPlayer CurrentPlayer { private set; get; }

        public int Compare(IGameEvent x, IGameEvent y)
        {
            if (x.Importance == 0 && y.Importance != 0)
                return 1;
            if (x.Importance != 0 && y.Importance == 0)
                return -1;
            if (x.Importance == 0 && y.Importance == 0)
                return 0;

            if (x.RelatesTo.Contains(CurrentPlayer) && !y.RelatesTo.Contains(CurrentPlayer))
                return 1;
            if (!x.RelatesTo.Contains(CurrentPlayer) && y.RelatesTo.Contains(CurrentPlayer))
                return -1;
            else
                return y.Importance.CompareTo(x.Importance);
        }
    }
}
