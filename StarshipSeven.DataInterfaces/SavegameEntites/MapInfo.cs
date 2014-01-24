using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.DataInterfaces.Exceptions;
using System.Runtime.Serialization;

namespace StarshipSeven.DataInterfaces.SavegameEntites
{
    [DataContract]
    public class MapInfo : BaseInfo
    {
        [DataMember]
        private List<PlanetInfo> _planets = new List<PlanetInfo>();

        [DataMember]
        public int MapWidth { get; set; }

        [DataMember]
        public int MapHeight { get; set; }

        public IEnumerable<PlanetInfo> Planets
        {
            get { return _planets; }
        }

        public void AddPlanet(PlanetInfo planet)
        {
            if (_planets.Contains(planet) || _planets.Any(x => x.Position.Value.Equals(planet.Position.Value)))
                throw new SavegameBuilderException("Savegame builder error.");
            planet.SavegameId = SavegameId;
            _planets.Add(planet);
        }
    }
}
