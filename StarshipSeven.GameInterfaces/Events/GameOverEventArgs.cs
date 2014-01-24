using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameInterfaces.Events
{
    public class GameOverEventArgs : EventArgs
    {
        public IEnumerable<IPlayer> Players { private set; get; }

        public IPlayer Winner { private set; get; }

        public GameOverEventArgs(IEnumerable<IPlayer> players, IPlayer winner)
        {
            Winner = winner;
            List<IPlayer> sortedPlayers = new List<IPlayer>(players.Count());
            sortedPlayers.Add(winner);
            var notDeadNeutral = players.SingleOrDefault(x => x is INeutralPlayer && !x.Statistics.DeathTurn.HasValue);
            if (notDeadNeutral != null)
                sortedPlayers.Add(notDeadNeutral);
            sortedPlayers.AddRange(players.Where(x => x.Statistics.DeathTurn.HasValue).OrderByDescending(x => x.Statistics.DeathTurn));
            Players = sortedPlayers;

        }
    }
}
