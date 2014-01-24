using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace StarshipSeven.DataInterfaces.SavegameEntites
{
    [DataContract]
    public class StatisticsInfo : BaseInfo
    {
        [DataMember]
        public Guid OwnerId { get; set; }

        [DataMember]
        public int ShipsProduced { get; set; }

        [DataMember]
        public int EnemyFleetsDestroyed { get; set; }

        [DataMember]
        public int FleetsLost { get; set; }

        [DataMember]
        public int EnemyShipsDestroyed { get; set; }

        [DataMember]
        public int? DeathTurn { get; set; }
    }
}
