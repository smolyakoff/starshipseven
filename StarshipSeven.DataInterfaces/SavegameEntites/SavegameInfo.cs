using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.DataInterfaces.Exceptions;
using System.Runtime.Serialization;

namespace StarshipSeven.DataInterfaces.SavegameEntites
{
    [DataContract]
    public class SavegameInfo
    {
        [DataMember]
        private List<PlayerInfo> _players = new List<PlayerInfo>();
        [DataMember]
        private List<FleetInfo> _movingFleets = new List<FleetInfo>();

        public SavegameInfo()
        {
            Id = Guid.NewGuid();
            MapInfo = new MapInfo();
            MapInfo.SavegameId = Id;
            TimeStamp = DateTime.Now;
        }

        [DataMember]
        public Guid Id {private set; get;}

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime TimeStamp { get; set; }

        [DataMember]
        public int CurrentTurn { get; set; }

        [DataMember]
        public Guid CurrenPlayerId { get; set; }

        [DataMember]
        public MapInfo MapInfo { private set; get; }

        public void AddPlayer(PlayerInfo player)
        {
            if (_players.Contains(player))
                throw new SavegameBuilderException("Savegame builder error.");
            player.SavegameId = Id;
            _players.Add(player);
        }

        public void AddFleet(FleetInfo fleet)
        {
            if (_movingFleets.Contains(fleet))
                throw new SavegameBuilderException("Savegame builder error.");
            fleet.SavegameId = Id;
            _movingFleets.Add(fleet);
        }

        public IEnumerable<PlayerInfo> Players
        {
            get { return _players; }
        }

        public IEnumerable<FleetInfo> MovingFleets
        {
            get { return _movingFleets; }
        }

    }
}
