using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Runtime.Serialization;

namespace StarshipSeven.DataInterfaces.SavegameEntites
{
    [DataContract]
    public class PlayerInfo : BaseInfo
    {
        [DataMember]
        private Guid _savegameId;
        private StatisticsInfo _statistics;

        public PlayerInfo()
        {
            Statistics = new StatisticsInfo();
        }

        [IgnoreDataMember]
        public override Guid SavegameId
        {
            get { return _savegameId; }
            internal set
            {
                _savegameId = value;
                if (Statistics == null)
                    Statistics = new StatisticsInfo();
                Statistics.SavegameId = value;
            }
        }

        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Color Color { get; set; }

        [DataMember]
        public PlayerType Type { get; set; }

        [DataMember]
        public string StrategyAssemblyQualifiedName { get; set; }

        [DataMember]
        public bool AutoEndTurnEnabled { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public StatisticsInfo Statistics
        {
            private set
            {
                _statistics = value;
                _statistics.OwnerId = Id;
            }
            get { return _statistics; }
        }
    }
}
