using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;
using System.Runtime.Serialization;

namespace StarshipSeven.DataInterfaces.SavegameEntites
{
    [DataContract]
    public class PlanetInfo : BaseInfo
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Position? Position { get; set; }

        [DataMember]
        public double KillPercentage { get; set; }

        [DataMember]
        public int ProductionRate { get; set; }

        [DataMember]
        public int Ships { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Guid OwnerId { get; set; }
    }
}
