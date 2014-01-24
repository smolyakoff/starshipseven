using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using StarshipSeven.GameInterfaces;
using StarshipSeven.GameInterfaces.Strategy;
using StarshipSeven.GameInterfaces.Events;
using StarshipSeven.GameEntities.Events;
using StarshipSeven.GameEntities.Exceptions;

namespace StarshipSeven.GameEntities
{
    public class Game : BaseGameEntity<Guid>, IGame
    {
        private int _currentTurn;
        private bool _isGameOver = false;
        private Player _currentPlayer;
        private Map _map;
        private List<Player> _players = new List<Player>();
        private List<Player> _deadPlayers = new List<Player>();
        private List<AttackFleet> _movingFleets = new List<AttackFleet>();
        private Queue<Player> _waitingPlayers = new Queue<Player>();
        private IBattleHandler _battlerHandler;

        #region IGame Members

        public Game() : base(Guid.NewGuid())
        {

        }

        public Game(Map map) : base(Guid.NewGuid())
        {
            SetProperty<Map>(ref _map, SetupMap(map), "Map");
        }

        public IPlayer CurrentPlayer
        {
            get { return _currentPlayer; }
        }

        public int CurrentTurn
        {
            get { return _currentTurn; }
            private set { SetProperty<int>(ref _currentTurn, value, "CurrentTurn"); }
        }

        public void Start()
        {
            if (_currentPlayer is ComputerPlayer)
                (_currentPlayer as ComputerPlayer).Think();
                
        }

        public IMap Map
        {
            get { return _map; }
        }

        public IEnumerable<IAttackFleet> MovingFleetsForCurrentPlayer
        {
            get { return _movingFleets.Where(x => x.Owner.Equals(_currentPlayer)); }
        }

        public ObservableCollection<IAttackFleet> FleetsToLaunchForCurrentPlayer
        {
            get { return _currentPlayer.FleetsToLaunch; }
        }

        public IBattleHandler BattleHandler
        {
            get { return _battlerHandler; }
            set { SetProperty<IBattleHandler>(ref _battlerHandler, value, "BattleHandler"); }
        }

        public event EventHandler BeforeTurnProcess;

        public event EventHandler<GameInterfaces.Events.PlanetChangedEventArgs> PlanetConquered;

        public event EventHandler<GameInterfaces.Events.PlanetChangedEventArgs> PlanetDefended;

        public event EventHandler<PlayerChangedEventArgs> PlayerDead;

        public event EventHandler<PlayerChangedEventArgs> PlayerDisqualified;

        public event EventHandler<GameOverEventArgs> GameOver;

        public event EventHandler<ReinforcementsArrivedEventArgs> ReinforcementsArrived;

        #endregion

        internal Game(int turn, Player currentPlayer, IEnumerable<Player> players, Map map, IEnumerable<AttackFleet> attackingFleets)
        {
            CurrentTurn = turn;
            SetMap(map);
            foreach (var player in players)
            {
                AttachPlayer(player);
                if (IsPlayerDead(player))
                {
                    _deadPlayers.Add(player);
                    _players.Remove(player);
                }
            }
            _movingFleets = new List<AttackFleet>(attackingFleets);
            foreach(var player in _players.SkipWhile(x => !x.Equals(currentPlayer)).Skip(1))
                _waitingPlayers.Enqueue(player);
            SetProperty<Player>(ref _currentPlayer, currentPlayer, "CurrentPlayer");
        }

        internal void Initialize()
        {
            //TODO : Add exception here
            CurrentTurn = 1;

            foreach (Player player in _players)
                _waitingPlayers.Enqueue(player);
            _map.ProcessNextTurn();
            SetProperty<Player>(ref _currentPlayer, _waitingPlayers.Dequeue(), "CurrentPlayer");
        }

        internal void SetMap(Map map)
        {
            if (_map != null)
                ReleaseMap(_map);
            SetProperty<Map>(ref _map, SetupMap(map), "Map");
        }

        private void OnBeforeTurnProcess()
        {
            if (BeforeTurnProcess != null)
                BeforeTurnProcess(this, new EventArgs());
        }

        private void OnPlanetConquered(object sender, PlanetChangedEventArgs args)
        {
            if (PlanetConquered != null)
                PlanetConquered(this, args);
        }

        private void OnPlanetDefended(object sender, PlanetChangedEventArgs args)
        {
            if (PlanetDefended != null)
                PlanetDefended(this, args);
        }

        private void OnReinforcementsArrived(object sender, ReinforcementsArrivedEventArgs args)
        {
            if (ReinforcementsArrived != null)
                ReinforcementsArrived(this, args);
        }


        private Map SetupMap(Map map)
        {
            foreach (Planet p in map)
            {
                p.Conquered += OnPlanetConquered;
                p.Defensed += OnPlanetDefended;
                p.ReinforcementsArrived += OnReinforcementsArrived;
                p.Battle += OnBattle;
            }
            return map;
        }

        private Map ReleaseMap(Map map)
        {
            foreach (Planet p in map)
            {
                p.Conquered -= OnPlanetConquered;
                p.Defensed -= OnPlanetDefended;
                p.ReinforcementsArrived -= OnReinforcementsArrived;
                p.Battle -= OnBattle;
            }
            return map;
        }

        private bool IsPlayerDead(Player player)
        {
            if (!player.ConqueredPlanets.Any() && !_movingFleets.Any(x => x.Owner.Equals(player)))
                return true;
            return false;
        }

        protected void OnEndTurn(object sender, EndTurnEventArgs args)
        {
            if (_isGameOver)
                return;

            foreach (AttackFleet fleet in args.LaunchedFleets)
                AddToMoving(fleet);
            if (_waitingPlayers.Count == 0)
                ProcessNextTurn();
            else
                SetProperty<Player>(ref _currentPlayer, _waitingPlayers.Dequeue(), "CurrentPlayer");

            if (_currentPlayer is ComputerPlayer && !_isGameOver)
            {
                try 
                { 
                    (_currentPlayer as ComputerPlayer).Think();
                }
                catch (CheatingException ex) 
                { 
                    OnDisqualify(ex.Cheater); 
                }
            }

            OnPropertyChanged("MovingFleetsForCurrentPlayer");
            OnPropertyChanged("Game");  
        }

        internal IEnumerable<Player> AllPlayers
        {
            get { return _players.Union(_deadPlayers); }
        }

        internal IEnumerable<AttackFleet> MovingFleets
        {
            get { return _movingFleets; }
        }

        private void OnBattle(object sender, BattleEventArgs args)
        {
            if (args.AttackFleet.Ships == 0)
                throw new Exception();
            BattleHandler.HandleBattle(args.AttackFleet, args.DefenseFleet);
        }

        private void OnDisqualify(Player cheater)
        {
            if (cheater == null)
                throw new GameLogicException("Cheating detected!");

            var neutralPlayer = _players.SingleOrDefault(x => x is NeutralPlayer);
            if (neutralPlayer == null)
            {
                neutralPlayer = _deadPlayers.SingleOrDefault(x => x is NeutralPlayer);
                _deadPlayers.Remove(neutralPlayer);
                _players.Add(neutralPlayer);
            }

            foreach (var planet in cheater.ConqueredPlanets)
                (planet as Planet).SetOwner(neutralPlayer);

            _players.Remove(cheater);
            _deadPlayers.Add(cheater);
            (cheater.Statistics as GameStatistics).DeathTurn = CurrentTurn;

            if (PlayerDisqualified != null)
                PlayerDisqualified(this, new PlayerChangedEventArgs(cheater));
        }

        private void ProcessNextTurn()
        {
            CurrentTurn += 1;
            OnBeforeTurnProcess();
            _map.ProcessNextTurn();

            Func<AttackFleet, bool> arrivesThisTurn = f => f.ArrivalTurn.Value == CurrentTurn;

            foreach (AttackFleet fleet in _movingFleets.Where(x => arrivesThisTurn(x)))
                (fleet.DestinationPlanet as Planet).MeetAttackFleet(fleet);
            _movingFleets.RemoveAll(x => arrivesThisTurn(x));

            foreach (Player player in _players.ToList())
            {
                if (IsPlayerDead(player))
                    OnPlayerDead(player);
                else
                    _waitingPlayers.Enqueue(player);
            }

            if (!_isGameOver)
                SetProperty<Player>(ref _currentPlayer, _waitingPlayers.Dequeue(), "CurrentPlayer");
        }

        private void OnPlayerDead(Player player)
        {
            _players.Remove(player);
            _deadPlayers.Add(player);
            (player.Statistics as GameStatistics).DeathTurn = CurrentTurn;
            if (PlayerDead != null)
                PlayerDead(this, new PlayerChangedEventArgs(player));

            if (_players.Count == 2 && _players.Any(x => x is NeutralPlayer))
                OnGameOver(_players.First(x => !(x is INeutralPlayer)));
        }

        private void OnGameOver(Player winner)
        {
            _isGameOver = true;
            if (GameOver != null)
                GameOver(this, new GameOverEventArgs(_players.Union(_deadPlayers), winner));
        }

        private void AddToMoving(AttackFleet fleet)
        {
            int arrivalTurn = CurrentTurn + fleet.SourcePlanet.GetDistanceTo(fleet.DestinationPlanet);
            fleet.ArrivalTurn = arrivalTurn;
            _movingFleets.Add(fleet);
        }

        public void AttachPlayer(Player player)
        {
            if (_players.Contains(player))
                throw new ItemDuplicateException(player, this);
            player.EndTurnEvent += OnEndTurn;
            player.Game = this;
            _players.Add(player);
        }
    }
}
