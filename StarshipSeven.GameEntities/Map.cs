using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using StarshipSeven.GameInterfaces;
using StarshipSeven.GameEntities.Exceptions;

namespace StarshipSeven.GameEntities
{
    public class Map : BaseGameEntity<Guid>, IMap
    {
        private SortedSet<Planet> _planets = new SortedSet<Planet>();
        private int _width;
        private int _height;

        public Map(int width, int height) : base(Guid.NewGuid())
        {
            Width = width;
            Height = height;
        }

        #region IMap Members

        public int Width
        {
            get { return _width; }
            set { SetProperty<int>(ref _width, value, "Width"); }
        }

        public int Height
        {
            get { return _height; }
            set { SetProperty<int>(ref _height, value, "Height"); }
        }

        public IEnumerator<IPlanet> GetEnumerator()
        {
            return _planets.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _planets.GetEnumerator();
        }

        public IPlanet GetPlanetOrNull(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException("Invalid planet position!");
            return this[x, y];
        }

        #endregion

        public Planet this[int x, int y]
        {
            get 
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    throw new ArgumentOutOfRangeException("Invalid planet position!");
                return _planets.SingleOrDefault(p => p.Position.Value.Equals(new Position(x, y))); 
            }
        }

        public void AddPlanet(Planet planet)
        {
            AddPlanet(planet, planet.Position.Value.X, planet.Position.Value.Y);
        }

        public void AddPlanet(Planet planet, int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException("Invalid planet position!");

            var existingPlanet = _planets.SingleOrDefault(p => p.Position.Value == new Position(x, y));

            if (existingPlanet != null)
                throw new ItemDuplicateException("There is already a planet in this position!");

            planet.Position = new Position(x, y);

            _planets.Add(planet);
        }

        public void RemovePlanet(Planet planet)
        {
            var planetToRemove = _planets.SingleOrDefault(p => p.Equals(planet));

            if (planetToRemove == null)
                throw new ItemNotFoundException(planet, this);

            planetToRemove.Position = null;

            _planets.Remove(planetToRemove);
        }

        internal void ProcessNextTurn()
        {
            foreach (Planet planet in this)
                planet.Produce();
        }

        internal IEnumerable<Planet> GetPlanetsForPlayer(Player player)
        {
            return _planets.Where(p => (p.Owner as Player).Equals(player));
        }

        internal IEnumerable<Planet> GetEnemyPlanetsForPlayer(Player player)
        {
            return _planets.Where(p => !(p.Owner as Player).Equals(player));
        }

        internal IEnumerable<Planet> Planets
        {
            get { return _planets; }
        }

        
    }
}
