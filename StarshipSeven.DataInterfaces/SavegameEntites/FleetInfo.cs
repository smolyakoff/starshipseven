using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace StarshipSeven.DataInterfaces.SavegameEntites
{
    [DataContract]
    public class FleetInfo : BaseInfo
    {
        [DataMember]
        public int Ships { get; set; }

        [DataMember]
        public Guid SourcePlanetId { get; set; }

        [DataMember]
        public Guid DestinationPlanetId { get; set; }

        [DataMember]
        public Guid OwnerId { get; set; }

        [DataMember]
        public int ArrivalTurn { get; set; }
    }
}
