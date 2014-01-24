using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Strategy.Attributes;

namespace StarshipSeven.GameInterfaces.Strategy
{
    public interface IStrategyInfo
    {
        string Title { get; }

        string Author { get; }

        string Tag { get; }

        DifficultyLevel Level { get; }

        IComputerTurnStrategy Strategy { get; }
    }
}
