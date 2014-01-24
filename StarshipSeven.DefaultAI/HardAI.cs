using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Strategy.Attributes;

namespace StarshipSeven.DefaultAI
{
    [Strategy(DifficultyLevel.Hard)]
    public class HardAI : BaseAI
    {

        protected override double RiskFactor
        {
            get { return 0.7; }
        }

        protected override double VisionFactor
        {
            get { return 1; }
        }

        protected override double ReinforcementsFactor
        {
            get { return 0.9; }
        }

        protected override double StupidityFactor
        {
            get { return 1; }
        }
    }
}
