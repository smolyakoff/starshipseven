using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;
using System.Windows.Media;

namespace StarshipSeven.GameEntities
{
    public class HumanPlayer : Player, IHumanPlayer
    {

        public HumanPlayer(Guid id)
            : base(id)
        {
        }

        public HumanPlayer(string name, Color color)
            :base(name, color)
        {

        }
    }
}
