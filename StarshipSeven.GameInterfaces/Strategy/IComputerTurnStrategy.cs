using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameInterfaces.Strategy
{
    public interface IComputerTurnStrategy
    {
        void MakeTurn(IComputerPlayer player);
    }
}
