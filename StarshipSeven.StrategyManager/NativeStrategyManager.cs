using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using StarshipSeven.DataInterfaces.Managers;
using StarshipSeven.GameInterfaces.Strategy;
using StarshipSeven.StrategyManager.Configuration;
using System.Configuration;
using System.Reflection;
using StarshipSeven.DataInterfaces.Exceptions;

namespace StarshipSeven.StrategyManager
{
    public class NativeStrategyManager : IStrategyManager
    {
        private List<StrategyInfo> _availableStrategies = new List<StrategyInfo>();

        public IStrategyManager LoadConfiguration()
        {
            _availableStrategies.Clear();
            StrategyManagerConfiguration config = ConfigurationManager.GetSection("strategyManager") as StrategyManagerConfiguration;
            DirectoryInfo directory = new DirectoryInfo(config.AssemblySource);
            if (!directory.Exists)
                throw new DirectoryNotFoundException("The source directory was not found!");
            foreach (var file in directory.GetFiles("*.dll"))
            {
                Assembly assembly;
                try
                {
                    assembly = Assembly.LoadFile(file.FullName);
                }
                catch (Exception)
                {
                    continue;
                }
                _availableStrategies.AddRange(StrategyInfo.GetStrategiesFromAssembly(assembly));
            }
            return this;
        }

        public IEnumerable<IStrategyInfo> AvailableStrategies
        {
            get { return _availableStrategies; }
        }

        public IComputerTurnStrategy GetByAssemblyQualifiedName(string name)
        {
            try
            {
                return _availableStrategies.Single(x => x.Strategy.GetType().AssemblyQualifiedName.Equals(name)).Strategy;
            }
            catch (Exception ex)
            {
                throw new ItemNotFoundException(name, this, ex);
            }
        }
    }
}
