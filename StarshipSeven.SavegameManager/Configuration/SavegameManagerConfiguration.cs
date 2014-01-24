using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace StarshipSeven.SavegameManager.Configuration
{
    public class SavegameManagerConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("savegameCatalog", IsRequired = true)]
        public string SavegameCatalog
        {
            get { return (string)base["savegameCatalog"]; }
            set { base["savegameCatalog"] = value; }

        }
    }
}
