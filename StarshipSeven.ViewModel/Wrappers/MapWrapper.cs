using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;
using System.Collections.ObjectModel;


namespace StarshipSeven.ViewModel.Wrappers
{
    public class MapWrapper : BaseViewModel
    {
        private IMap _map;
        private MapTile _currentSourcePlanet;
        private MapTile _currentDestinationPlanet;

        public MapWrapper(IMap map)
        {
            Map = map;
        }

        public IMap Map
        {
            get { return _map; }
            set
            {
                SetProperty<IMap>(ref _map, value, "Map");
                OnPropertyChanged("Tiles");
            }
        }

        private IEnumerable<MapTile> CreateTiles()
        {
            for (int i = 0; i < Map.Width; i++)
                for (int j = 0; j < Map.Height; j++)
                    yield return new MapTile(Map.GetPlanetOrNull(i, j));
        }

        public MapTile CurrentSourcePlanet
        {
            get { return _currentSourcePlanet; }
            set
            {
                if (_currentSourcePlanet != value && _currentSourcePlanet != null)
                    _currentSourcePlanet.IsSourcePlanet = false;
                if (value != null)
                    value.IsSourcePlanet = true;
                SetProperty<MapTile>(ref _currentSourcePlanet, value, "CurrentSourcePlanet");
                if (CurrentSourcePlanet == CurrentDestinationPlanet && value != null)
                    CurrentDestinationPlanet = null; 
            }
        }

        public MapTile CurrentDestinationPlanet
        {
            get { return _currentDestinationPlanet; }
            set
            {
                if (_currentDestinationPlanet != value && _currentDestinationPlanet != null)
                    _currentDestinationPlanet.IsDestinationPlanet = false;
                if (value != null)
                    value.IsDestinationPlanet = true;
                SetProperty<MapTile>(ref _currentDestinationPlanet, value, "CurrentDestinationPlanet");
                if (CurrentSourcePlanet == CurrentDestinationPlanet && value != null)
                    CurrentSourcePlanet = null; 
            }
        }

        public IEnumerable<MapTile> Tiles
        {
            get
            {
                if (Map == null)
                    return new List<MapTile>();
                return CreateTiles(); 
            }
        }
    }

}
