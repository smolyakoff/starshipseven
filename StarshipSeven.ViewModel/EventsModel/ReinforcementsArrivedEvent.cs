using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Events;
using StarshipSeven.GameInterfaces;

namespace StarshipSeven.ViewModel.EventsModel
{
    public class ReinforcementsArrivedEvent : BaseEvent<ReinforcementsArrivedEventArgs>
    {
        private const string _msg = "Reinforcements ({0} ships) arrived to planet \"{1}\".";

        public ReinforcementsArrivedEvent(ReinforcementsArrivedEventArgs args)
            : base(args)
        {
        }

        public override string Message
        {
            get { return string.Format(_msg, _eventArgs.Ships, _eventArgs.Planet.Name); }
        }

        public override IEnumerable<IPlayer> RelatesTo
        {
            get {return AsEnumerable<IPlayer>(_eventArgs.Planet.Owner); }
        }

        public override int Importance
        {
            get { return 3; }
        }
    }
}
