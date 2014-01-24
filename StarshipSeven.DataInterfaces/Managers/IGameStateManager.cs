using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;
using StarshipSeven.DataInterfaces.SavegameEntites;

namespace StarshipSeven.DataInterfaces.Managers
{
    public interface IGameStateManager
    {
        IStrategyManager StrategyManager { get; set; }

        SavegameInfo SaveState(IGame game);
        IGame LoadState(SavegameInfo savegame);
    }
}
