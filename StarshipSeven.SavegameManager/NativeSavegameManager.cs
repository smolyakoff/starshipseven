using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.DataInterfaces.Managers;
using StarshipSeven.DataInterfaces.SavegameEntites;
using StarshipSeven.SavegameManager.Configuration;
using System.Configuration;
using System.IO;
using StarshipSeven.DataInterfaces.Exceptions;
using System.Runtime.Serialization;

namespace StarshipSeven.SavegameManager
{
    public class NativeSavegameManager : ISavegameManager
    {
        private DirectoryInfo _savegameCatalog;

        private const string _extension = ".ssg";

        public NativeSavegameManager()
        {
            LoadConfiguration();
        }

        private string ConstructPath(string savegameName)
        {
            return Path.Combine(_savegameCatalog.FullName, savegameName + _extension);
        }

        private void LoadConfiguration()
        {
            SavegameManagerConfiguration config = (SavegameManagerConfiguration)ConfigurationManager.GetSection("savegameManager");
            DirectoryInfo dir = new DirectoryInfo(config.SavegameCatalog);
            if (!dir.Exists)
            {
                try
                {
                    dir.Create();
                }
                catch (Exception ex)
                {
                    throw new DataConnectionException("Can't initialize savegame catalog.", ex);
                }
            }
            _savegameCatalog = dir; 
        }

        public bool Exists(string name)
        {
            return _savegameCatalog.GetFiles("*"+_extension).Any(x => Path.GetFileNameWithoutExtension(x.Name).Equals(name));
        }

        public void Save(SavegameInfo savegame)
        {
            using (var stream = new FileStream(ConstructPath(savegame.Name), FileMode.Create))
            {
                var serializer = new DataContractSerializer(typeof(SavegameInfo));
                try
                {
                    serializer.WriteObject(stream, savegame);
                }
                catch (Exception ex)
                {
                    throw new DataConnectionException("Can't save the game.", ex);
                }
            }
        }

        public SavegameInfo Load(string name)
        {
            FileStream stream;
            try
            {
                stream = File.Open(ConstructPath(name), FileMode.Open);
            }
            catch (FileNotFoundException ex)
            {
                throw new ItemNotFoundException(name, this, ex);
            }
            catch (Exception ex)
            {
                throw new DataException("Can't load the game.", ex);
            }
            using (stream)
            {
                var serializer = new DataContractSerializer(typeof(SavegameInfo));
                try
                {
                    return (SavegameInfo)serializer.ReadObject(stream);
                }
                catch (Exception ex)
                {
                    throw new DataException("Can't load the game.", ex);
                }
            }
        }


        public IEnumerable<SavegameInfo> LoadAll()
        {
            foreach (var path in _savegameCatalog.GetFiles('*' + _extension).Select(x => Path.GetFileNameWithoutExtension(x.Name)))
                yield return Load(path);
        }


        public void Delete(string name)
        {
            string path = ConstructPath(name);
            if (!File.Exists(path))
                throw new ItemNotFoundException(name, this, null);
            try
            {
                File.Delete(path);
            }
            catch (Exception ex)
            {
                throw new DataConnectionException("Can't delete the savgame.", ex);
            }
        }
    }
}
