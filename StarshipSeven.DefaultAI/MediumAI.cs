using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Strategy.Attributes;

namespace StarshipSeven.DefaultAI
{
    [Strategy(DifficultyLevel.Medium)]
    public class MediumAI : BaseAI
    {

        protected override double RiskFactor
        {
            get { return 0.8; }
        }

        protected override double VisionFactor
        {
            get { return 0.7; }
        }

        protected override double ReinforcementsFactor
        {
            get { return 0.6; }
        }

        protected override double StupidityFactor
        {
            get { return 0.5; }
        }
    }
}
