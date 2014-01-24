using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Strategy;

namespace StarshipSeven.DataInterfaces.Managers
{
    public interface IStrategyManager
    {
        IStrategyManager LoadConfiguration();
        IEnumerable<IStrategyInfo> AvailableStrategies { get; }
        IComputerTurnStrategy GetByAssemblyQualifiedName(string name);
    }
}
