using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameInterfaces.Strategy.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StrategyAttribute : Attribute
    {
        public StrategyAttribute(DifficultyLevel level)
        {
            Level = level;
        }

        public DifficultyLevel Level { get; set; }

        public string Tag { get; set; }
    }
}
