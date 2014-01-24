using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameInterfaces.Strategy.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class StrategySetAttribute : Attribute
    {
        public StrategySetAttribute(string title)
        {
            Title = title;
        }

        public string Title { get; set; }

        public string Author { get; set; }
    }
}
