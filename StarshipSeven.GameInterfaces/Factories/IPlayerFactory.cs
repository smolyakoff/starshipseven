using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Strategy;
using System.Windows.Media;

namespace StarshipSeven.GameInterfaces.Factories
{
    public interface IPlayerFactory
    {
        IHumanPlayer CreateHumanPlayer(string name, Color color);
        IComputerPlayer CreateComputerPlayer(string name, Color color, IComputerTurnStrategy strategy, bool autoEndTurn);
        INeutralPlayer CreateNeutralPlayer();
    }
}
