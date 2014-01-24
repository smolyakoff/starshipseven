using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Events;
using StarshipSeven.GameInterfaces;

namespace StarshipSeven.ViewModel.EventsModel
{
    public class PlayerDeadEvent : BaseEvent<PlayerChangedEventArgs>
    {
        private const string _msg = "The glorious empire of {0} has fallen!";

        public PlayerDeadEvent(PlayerChangedEventArgs args)
            : base(args)
        {

        }

        public override string Message
        {
            get { return string.Format(_msg, _eventArgs.Player.Name); }
        }

        public override IEnumerable<IPlayer> RelatesTo
        {
            get { return AsEnumerable<IPlayer>(_eventArgs.Player); }
        }

        public override int Importance
        {
            get { return 0; }
        }
    }
}
