using System;

namespace StarshipSeven.GameInterfaces
{
    public interface IGameStatistics : IGameEntity
    {
        int EnemyFleetsDestroyed { get; }
        int EnemyShipsDestroyed { get; }
        int FleetsLost { get; }
        int ShipsProduced { get; }
        int? DeathTurn { get; }
    }
}