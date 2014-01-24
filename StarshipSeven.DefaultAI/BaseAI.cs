using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Strategy;
using StarshipSeven.GameInterfaces;
using StarshipSeven.GameInterfaces.Strategy.Attributes;

[assembly: StrategySet("Default AI", Author = "K. Smolyakov")]

namespace StarshipSeven.DefaultAI
{
    public abstract class BaseAI : IComputerTurnStrategy
    {
        private IComputerPlayer _player;

        public void MakeTurn(IComputerPlayer player)
        {
            _player = player;
            foreach (IPlanet myPlanet in _player.ConqueredPlanets)
            {
                var attackTarget = ChooseTargetPlanet(myPlanet);
                if (attackTarget == null)
                {
                    var reinforcementsTarget = ChoosePlanetForReinforcements(myPlanet);
                    if (reinforcementsTarget != null)
                    {
                        int shipsToLaunch = CanLaunchForReinforcements(myPlanet);
                        if (shipsToLaunch > 0)
                            player.LaunchFleet(shipsToLaunch, myPlanet, reinforcementsTarget);
                    }
                }
                else
                {
                    int shipsToLaunch = CanLaunch(myPlanet);
                    if (shipsToLaunch > 0)
                        player.LaunchFleet(shipsToLaunch, myPlanet, attackTarget);
                }
                    
            }
        }

        private IPlanet ChooseTargetPlanet(IPlanet myPlanet)
        {
            var targets = GetVisibleEnemyPlanets(myPlanet)
                          .Where(x => CanWin(myPlanet, x))
                          .OrderBy(x => myPlanet.GetDistanceTo(x));
            var neutral = targets.FirstOrDefault(x => x.Owner is INeutralPlayer);
            return (neutral == null) ? targets.FirstOrDefault() : neutral;
        }


        private IPlanet ChoosePlanetForReinforcements(IPlanet myPlanet)
        {
            var target = GetVisibleConqueredPlanets(myPlanet).OrderByDescending(x => x.KillPercentage).FirstOrDefault();
            if (target == null)
                return null;
            return (target.KillPercentage < myPlanet.KillPercentage) ? null : target;
        }

        private int EnemyPlanetsCount
        {
            get { return _player.EnemyPlanets.Count(); }
        }

        private IEnumerable<IPlanet> GetVisibleConqueredPlanets(IPlanet myPlanet)
        {
            int visiblePlanets = Convert.ToInt32(EnemyPlanetsCount * VisionFactor);
            return _player.ConqueredPlanets.Where(x => !x.Equals(myPlanet)).OrderBy(x => myPlanet.GetDistanceTo(x)).Take(visiblePlanets);
        }

        private IEnumerable<IPlanet> GetVisibleEnemyPlanets(IPlanet myPlanet)
        {
            int visiblePlanets = Convert.ToInt32(EnemyPlanetsCount * VisionFactor);
            return _player.EnemyPlanets.OrderBy(x => myPlanet.GetDistanceTo(x)).Take(visiblePlanets);
        }

        private bool CanWin(IPlanet sourcePlanet, IPlanet enemyPlanet)
        {
            int distance = sourcePlanet.GetDistanceTo(enemyPlanet);

            return GetMyPower(sourcePlanet) / ExpectedEnemyPower(enemyPlanet, distance) >= StupidityFactor;
        }

        private double GetMyPower(IPlanet sourcePlanet)
        {
            return CanLaunch(sourcePlanet) * sourcePlanet.KillPercentage;
        }

        private double ExpectedEnemyPower(IPlanet planet, int distance)
        {
            return (planet.Ships + distance * planet.ProductionRate) * planet.KillPercentage;
        }

        private int CanLaunch(IPlanet planet)
        {
            return Convert.ToInt32(planet.Ships * RiskFactor);
        }

        private int CanLaunchForReinforcements(IPlanet planet)
        {
            return Convert.ToInt32(planet.Ships * ReinforcementsFactor);
        }

        protected abstract double StupidityFactor { get; }

        protected abstract double RiskFactor { get; }

        protected abstract double VisionFactor { get; }

        protected abstract double ReinforcementsFactor { get; }

    }
}
