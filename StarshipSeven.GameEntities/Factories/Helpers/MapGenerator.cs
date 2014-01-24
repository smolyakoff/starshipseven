using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;

namespace StarshipSeven.GameEntities.Factories.Helpers
{
    public class MapGenerator
    {
        private Random _random = new Random();
        private const int  _maxProductionRate = 16;
        private const int _minProductionRate = 3;
        private const double _minKillPercentage = 0.3;
        private const double _maxKillPercentage = 0.9;
        private const int _minProductionRateForPlayer = 9;
        private const int _maxProductionRateForPlayer = 13;
        private const double _minKillPercentageForPlayer = 0.5;
        private const double _maxKillPercentageForPlayer = 0.8;


        public Map Generate(int width, int height, IEnumerable<Player> players, int neutralPlanetsCount, NeutralPlayer neutralPlayer)
        {
            Map map = new Map(width, height);
            int cellsCount = width * height;
            int planetsCount = players.Count() + neutralPlanetsCount;
            int i = 0;
            foreach (Segment segment in DivideIntoRandomSegments(cellsCount, planetsCount))
            {
                int cell = _random.Next(segment.Left, segment.Right);
                Planet p = new Planet(GetPlanetName(i++));
                RandomPlanetSetup(p);
                Position pos = ConvertCellToPosition(cell, width);
                map.AddPlanet(p, pos.X, pos.Y);
            }

            i = 0;
            foreach (Segment segment in DivideIntoRandomSegments(planetsCount, players.Count()))
            {
                int planetNo = _random.Next(segment.Left, segment.Right);
                var planet = map.Planets.ElementAt(planetNo);
                PlayerPlanetSetup(planet);
                planet.SetOwner(players.ElementAt(i++));
            }

            if (neutralPlanetsCount != 0)
                foreach (Planet planet in map.Planets.Where(x => x.Owner == null))
                    planet.SetOwner(neutralPlayer);

            return map;
        }

        private void PlayerPlanetSetup(Planet planet)
        {
            planet.ProductionRate = _random.Next(_minProductionRateForPlayer, _maxProductionRateForPlayer);
            double x = _random.NextDouble();
            x = (x < _minKillPercentageForPlayer) ? _minKillPercentageForPlayer : x;
            x = (x > _maxKillPercentageForPlayer) ? _maxKillPercentageForPlayer : x;
            planet.KillPercentage = x;
        }

        private void RandomPlanetSetup(Planet planet)
        {
            planet.ProductionRate = _random.Next(_minProductionRate, _maxProductionRate);
            double x = _random.NextDouble();
            x = (x < _minKillPercentage) ? _minKillPercentage : x;
            x = (x > _maxKillPercentage) ? _maxKillPercentage : x;
            planet.KillPercentage = x;
        }
        

        private string GetPlanetName(int number)
        {
            int lettersCount = 'Z' - 'A' + 1;
            if (number < lettersCount)
                return Convert.ToChar('A' + number).ToString();
            else
                return GetPlanetName(number / lettersCount - 1) + GetPlanetName(number % lettersCount);
        }

        private Position ConvertCellToPosition(int cell, int width)
        {
            int y = cell / width;
            int x = cell % width;
            return new Position(x, y);
        }

        private Segment GetRandomSegment(int left, int maxRight)
        {
            int right = _random.Next(left + 1, maxRight);
            return new Segment(left, right);
        }

        private void DivideIntoTwoRandomSegments(Segment segment, out Segment leftSegment, out Segment rightSegment)
        {
            leftSegment = GetRandomSegment(segment.Left, segment.Right);
            rightSegment = new Segment(leftSegment.Right, segment.Right);
        }

        private IEnumerable<Segment> DivideIntoRandomSegments(int cellCount, int segmentsCount)
        {
            if (segmentsCount == 0)
                return new List<Segment>();

            List<Segment> segments = new List<Segment>(segmentsCount);
            segments.Add(new Segment(0, cellCount));

            while (segments.Count < segmentsCount)
            {
                Segment maxSegment = segments.OrderBy(x => x.Length).Last();
                Segment l, r;
                DivideIntoTwoRandomSegments(maxSegment, out l, out r);
                segments.Remove(maxSegment);
                segments.Add(l);
                segments.Add(r);
            }

            return segments;
        }

    }
}
