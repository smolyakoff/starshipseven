using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Events;
using StarshipSeven.GameInterfaces;

namespace StarshipSeven.ViewModel.EventsModel
{
    public class PlanetConqueredEvent : BaseEvent<PlanetChangedEventArgs>
    {
        private string _msg = "The planet \"{0}\" was conquered by {1}.";

        public PlanetConqueredEvent(PlanetChangedEventArgs args)
            : base(args)
        {
        }

        public override string Message
        {
            get { return string.Format(_msg, _eventArgs.ChangedPlanet.Name, _eventArgs.AttackingPlayer.Name); }
        }

        public override IEnumerable<IPlayer> RelatesTo
        {
            get { return AsEnumerable<IPlayer>(_eventArgs.AttackingPlayer, _eventArgs.DefendingPlayer); }
        }

        public override int Importance
        {
            get { return 1; }
        }
    }
}
