using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.DataInterfaces.SavegameEntites;

namespace StarshipSeven.DataInterfaces.Managers
{
    public interface ISavegameManager
    {
        bool Exists(string name);
        void Save(SavegameInfo savegame);
        IEnumerable<SavegameInfo> LoadAll();
        SavegameInfo Load(string name);
        void Delete(string name);
    }
}
