using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;
using System.Windows.Media;

namespace StarshipSeven.GameEntities
{
    public class NeutralPlayer : ComputerPlayer, INeutralPlayer
    {
        public NeutralPlayer()
            :base("Neutral Player", Colors.Gray, null, true)
        {
        }

        public NeutralPlayer(Guid id)
            : base(id)
        {
        }

        public override void Think()
        {
            //Neutral player just do nothing :)
            OnEndTurn();
        }
    }
}
