using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Factories;
using StarshipSeven.GameEntities.Factories;
using StarshipSeven.GameInterfaces;
using StarshipSeven.GameEntities;
using StarshipSeven.GameInterfaces.Events;
using StarshipSeven.DefaultAI;
using System.Collections.ObjectModel;
using System.Windows.Media;
using StarshipSeven.DataInterfaces.Managers;
using StarshipSeven.GameEntities.Managers;
using StarshipSeven.SavegameManager;
using StarshipSeven.StrategyManager;

namespace StarshipSeven.GameFactoryTest
{
    class Program
    {
        static IGame globalGame;

        static void Main(string[] args)
        {
            IGameFactory factory = new GameFactory();
            ObservableCollection<IPlayer> players = new ObservableCollection<IPlayer>();
            players.Add(factory.PlayerFactory.CreateComputerPlayer("CPU1", Colors.White, new EasyAI(), true));
            players.Add(factory.PlayerFactory.CreateComputerPlayer("CPU2", Colors.Black, new MediumAI(), true));
            players.Add(factory.PlayerFactory.CreateComputerPlayer("CPU3", Colors.White, new HardAI(), true));
            factory.Players = players;

            factory.MapWidth = 30;
            factory.MapHeight = 30;

            factory.NeutralPlanetsNumber = 200;
            factory.BattleHandler = new DefaultBattleHandler.DefaultBattleHandler();

            IGame game = factory.Construct();

            game.PlanetConquered += OnPlanetConquered;
            game.PlanetDefended += OnPlanetDefended;
            game.ReinforcementsArrived += OnReinforcementsArrived;
            game.PlayerDead += OnPlayerDead;
            game.GameOver += OnGameOver;

            globalGame = game;
            game.Start();

            IGameStateManager gameStateManager = new GameStateManager();
            gameStateManager.StrategyManager = new NativeStrategyManager().LoadConfiguration();
            var save = gameStateManager.SaveState(game);
            ISavegameManager savegameManager = new NativeSavegameManager();
            save.Name = "game1";
            savegameManager.Save(save);

            var load = savegameManager.Load("game1");
            IGame loadedGame = gameStateManager.LoadState(load);

            loadedGame.Start();
            

            Console.ReadLine();       
        }

        static void OnGameOver(object sender, GameOverEventArgs args)
        {
            var map = globalGame.Map as Map;
            for (int i = 0; i < globalGame.Map.Width; i++)
            {
                for (int j = 0; j < globalGame.Map.Height; j++)
                    Console.Write((map[i, j] == null) ? "0 " : map[i, j].Owner.Name.Last().ToString() + " ");
                Console.WriteLine();
            }
            Console.WriteLine();
            foreach (Player player in args.Players)
            {
                
                Console.WriteLine("Name: {0}", player.Name);
                Console.WriteLine("Ships produced: {0}", player.Statistics.ShipsProduced);
                Console.WriteLine("Enemy ships destroyed: {0}", player.Statistics.EnemyShipsDestroyed);
                Console.WriteLine("Enemy fleets destroyed: {0}", player.Statistics.EnemyFleetsDestroyed);
                Console.WriteLine("Losts: {0}", player.Statistics.FleetsLost);
                Console.WriteLine("Death Turn: {0}", player.Statistics.DeathTurn);
                Console.WriteLine();
            }              
        }

        static void OnReinforcementsArrived(object sender, ReinforcementsArrivedEventArgs args)
        {
            Console.WriteLine("Reinforcements ({0} ships) arrived to {1} by {2}", args.Ships, args.Planet.Name, args.Planet.Owner.Name);
        }

        static void OnPlayerDead(object sender, PlayerChangedEventArgs args)
        {
            Console.WriteLine("{0} is dead", args.Player.Name);
        }

        static void OnPlanetConquered(object sender, PlanetChangedEventArgs args)
        {
            Console.WriteLine("Planet {0} conquered by {1}", args.ChangedPlanet.Name, args.ChangedPlanet.Owner.Name); 
        }

        static void OnPlanetDefended(object sender, PlanetChangedEventArgs args)
        {
            Console.WriteLine("Planet {0} defended by {1}", args.ChangedPlanet.Name, args.ChangedPlanet.Owner.Name);
        }



            
    }
}
