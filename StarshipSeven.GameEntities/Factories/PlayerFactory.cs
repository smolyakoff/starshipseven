using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Factories;
using StarshipSeven.GameInterfaces;
using StarshipSeven.GameInterfaces.Strategy;
using System.Windows.Media;

namespace StarshipSeven.GameEntities.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        public IHumanPlayer CreateHumanPlayer(string name, Color color)
        {
            return new HumanPlayer(name, color);
        }

        public IComputerPlayer CreateComputerPlayer(string name, Color color, IComputerTurnStrategy strategy, bool autoEndTurn)
        {
            return new ComputerPlayer(name, color, strategy, autoEndTurn);
        }

        public INeutralPlayer CreateNeutralPlayer()
        {
            return new NeutralPlayer();
        }
    }
}
