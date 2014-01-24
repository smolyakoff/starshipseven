using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace StarshipSeven.DataInterfaces.SavegameEntites
{
    [DataContract]
    public abstract class BaseInfo
    {
        [DataMember]
        public virtual Guid SavegameId { get; internal set; }
    }
}
