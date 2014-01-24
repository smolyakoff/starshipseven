using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace StarshipSeven.StrategyManager.Configuration
{
    public class StrategyManagerConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("assemblySource", IsRequired = true)]
        public string AssemblySource
        {
            get { return (string) base["assemblySource"]; }
            set { base["assemblySource"] = value; }

        }
    }
}
