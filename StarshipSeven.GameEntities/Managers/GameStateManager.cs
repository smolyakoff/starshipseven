using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.DataInterfaces.Managers;
using StarshipSeven.DataInterfaces.SavegameEntites;
using StarshipSeven.GameInterfaces;
using StarshipSeven.GameEntities.Exceptions;
using StarshipSeven.DataInterfaces.Exceptions;

namespace StarshipSeven.GameEntities.Managers
{
    public class GameStateManager : IGameStateManager
    {
        public SavegameInfo SaveState(IGame game)
        {
            if (!(game is Game))
                throw new TypeMismatchException(typeof(Game), game);
            Game gameObject = game as Game;
            SavegameInfo save = new SavegameInfo()
            {
                CurrentTurn = gameObject.CurrentTurn,
                TimeStamp = DateTime.Now,
                CurrenPlayerId = (gameObject.CurrentPlayer as Player).Id
            };
            save.MapInfo.MapHeight = gameObject.Map.Height;
            save.MapInfo.MapWidth = gameObject.Map.Width;
            foreach (var planet in gameObject.Map)
                save.MapInfo.AddPlanet(ConvertPlanet(planet));
            int i = 0;
            foreach (var player in gameObject.AllPlayers)
                save.AddPlayer(ConvertPlayer(player, i++));
            foreach (var fleet in gameObject.MovingFleets)
                save.AddFleet(ConvertFleet(fleet));
            return save;
        }


        public IGame LoadState(SavegameInfo savegame)
        {
            List<Player> players = new List<Player>(savegame.Players.Count());
            List<AttackFleet> fleets = new List<AttackFleet>(savegame.MovingFleets.Count());
            foreach (PlayerInfo info in savegame.Players)
                players.Insert(info.Order, ConvertPlayerBack(info));
            Map map = new Map(savegame.MapInfo.MapWidth, savegame.MapInfo.MapHeight);
            foreach (PlanetInfo planetInfo in savegame.MapInfo.Planets)
                map.AddPlanet(ConvertPlanetBack(planetInfo, players));
            foreach (FleetInfo fleetInfo in savegame.MovingFleets)
                fleets.Add(ConvertFleetBack(fleetInfo, players, map));
            var currentPlayer = players.SingleOrDefault(x => x.Id == savegame.CurrenPlayerId);
            if (currentPlayer == null)
                throw new SavegameBuilderException("Savegame corrupted");
            Game game = new Game(savegame.CurrentTurn, currentPlayer, players, map, fleets);
            return game;
        }

        private AttackFleet ConvertFleetBack(FleetInfo fleetInfo, IEnumerable<Player> players, Map map)
        {
            try
            {
                var owner = players.SingleOrDefault(x => x.Id == fleetInfo.OwnerId);
                var sourcePlanet = map.Planets.SingleOrDefault(x => x.Id == fleetInfo.SourcePlanetId);
                var destinationPlanet = map.Planets.SingleOrDefault(x => x.Id == fleetInfo.DestinationPlanetId);
                var fleet = new AttackFleet(owner, fleetInfo.Ships, sourcePlanet, destinationPlanet);
                sourcePlanet.SetupAttackFleet(fleet);
                fleet.ArrivalTurn = fleetInfo.ArrivalTurn;
                return fleet;
            }
            catch (Exception)
            {
                throw new SavegameBuilderException("Savegame corrupted.");
            }
        }

        private Planet ConvertPlanetBack(PlanetInfo planetInfo, IEnumerable<Player> players)
        {
            Planet planet = new Planet(planetInfo.Id)
            {
                Name = planetInfo.Name,
                Position = planetInfo.Position,
                KillPercentage = planetInfo.KillPercentage,
                ProductionRate = planetInfo.ProductionRate,
                Ships = planetInfo.Ships,
            };
            var owner = players.SingleOrDefault(x => x.Id == planetInfo.OwnerId);
            if (owner == null)
                throw new SavegameBuilderException("Corrupted savegame.");
            planet.SetOwner(owner);
            return planet;
        }

        private Player ConvertPlayerBack(PlayerInfo playerInfo)
        {
            Player player;
            if (playerInfo.Type == PlayerType.HumanPlayer)
                player = new HumanPlayer(playerInfo.Id);
            else if (playerInfo.Type == PlayerType.NeutralPlayer)
                player = new NeutralPlayer(playerInfo.Id);
            else
            {
                player = new ComputerPlayer(playerInfo.Id);
                if (StrategyManager == null)
                    throw new SavegameBuilderException("Strategy Manager is not initialized!");
                (player as ComputerPlayer).Strategy = StrategyManager.GetByAssemblyQualifiedName(playerInfo.StrategyAssemblyQualifiedName);
                (player as ComputerPlayer).AutoEndTurnEnabled = playerInfo.AutoEndTurnEnabled;
            }
            (player.Statistics as GameStatistics).DeathTurn = playerInfo.Statistics.DeathTurn;
            (player.Statistics as GameStatistics).EnemyFleetsDestroyed = playerInfo.Statistics.EnemyFleetsDestroyed;
            (player.Statistics as GameStatistics).EnemyShipsDestroyed = playerInfo.Statistics.EnemyShipsDestroyed;
            (player.Statistics as GameStatistics).FleetsLost = playerInfo.Statistics.FleetsLost;
            (player.Statistics as GameStatistics).ShipsProduced = playerInfo.Statistics.ShipsProduced;
            player.Name = playerInfo.Name;
            player.Color = playerInfo.Color;
            return player;
        }

        private PlanetInfo ConvertPlanet(IPlanet planet)
        {
            PlanetInfo planetInfo = new PlanetInfo()
            {
                Id = (planet as Planet).Id,
                Name = planet.Name,
                KillPercentage = planet.KillPercentage,
                Position = planet.Position,
                ProductionRate = planet.ProductionRate,
                Ships = planet.Ships,
                OwnerId = (planet.Owner as Player).Id
            };
            return planetInfo;
        }

        private PlayerInfo ConvertPlayer(Player player, int order)
        {
            PlayerInfo playerInfo = new PlayerInfo()
            {
                Id = player.Id,
                Name = player.Name,
                Color = player.Color,
                Order = order
            };
            playerInfo.Statistics.DeathTurn = player.Statistics.DeathTurn;
            playerInfo.Statistics.EnemyFleetsDestroyed = player.Statistics.EnemyFleetsDestroyed;
            playerInfo.Statistics.EnemyShipsDestroyed = player.Statistics.EnemyShipsDestroyed;
            playerInfo.Statistics.FleetsLost = player.Statistics.FleetsLost;
            playerInfo.Statistics.ShipsProduced = player.Statistics.ShipsProduced;
            if (player is NeutralPlayer)
                playerInfo.Type = PlayerType.NeutralPlayer;
            else if (player is HumanPlayer)
                playerInfo.Type = PlayerType.HumanPlayer;
            else
            {
                playerInfo.Type = PlayerType.ComputerPlayer;
                playerInfo.StrategyAssemblyQualifiedName = (player as ComputerPlayer).Strategy.GetType().AssemblyQualifiedName;
                playerInfo.AutoEndTurnEnabled = (player as ComputerPlayer).AutoEndTurnEnabled;
            }
            return playerInfo;
        }

        private FleetInfo ConvertFleet(AttackFleet fleet)
        {
            FleetInfo fleetInfo = new FleetInfo()
            {
                SourcePlanetId = (fleet.SourcePlanet as Planet).Id,
                DestinationPlanetId = (fleet.DestinationPlanet as Planet).Id,
                OwnerId = fleet.Owner.Id,
                Ships = fleet.Ships,
                ArrivalTurn = fleet.ArrivalTurn.Value
            };
            return fleetInfo;
        }

        public IStrategyManager StrategyManager { get; set; }

    }
}
