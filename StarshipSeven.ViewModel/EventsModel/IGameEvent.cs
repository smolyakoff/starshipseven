using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;

namespace StarshipSeven.ViewModel.EventsModel
{
    public interface IGameEvent
    {
        string Message { get; }

        IEnumerable<IPlayer> RelatesTo { get; }

        int Importance { get; }
    }
}
