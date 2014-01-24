using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Factories;
using StarshipSeven.GameInterfaces;
using StarshipSeven.GameInterfaces.Strategy;
using StarshipSeven.GameEntities.Factories.Helpers;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace StarshipSeven.GameEntities.Factories
{
    public class GameFactory : BaseGameFactory
    {
        private MapGenerator _mapGenerator = new MapGenerator();
        private Game _game = new Game();
        private NeutralPlayer _neutralPlayer;
        private Map _map;

        public GameFactory() : base(new PlayerFactory())
        {
            _neutralPlayer = PlayerFactory.CreateNeutralPlayer() as NeutralPlayer;
        }

        public override void GenerateMap()
        {
            _map = _mapGenerator.Generate(MapWidth, MapHeight, Players.OfType<Player>(), NeutralPlanetsNumber, _neutralPlayer);
            base.GenerateMap();
        }

        public override IMap CurrentMap
        {
            get { return _map; }
        }

        public override IGame Construct()
        {
            if (_map == null)
                GenerateMap();
            if (NeutralPlanetsNumber != 0)
                _game.AttachPlayer(_neutralPlayer);
            foreach (var player in Players.OfType<Player>())
                _game.AttachPlayer(player);
            _game.SetMap(_map);
            _game.BattleHandler = this.BattleHandler;
            _game.Initialize();
            return _game;
        }
    }
}
