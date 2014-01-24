using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Strategy.Attributes;
using System.Reflection;

namespace StarshipSeven.GameInterfaces.Strategy
{
    public class StrategyInfo : IStrategyInfo
    {
        private StrategySetAttribute _assemblyAttribute;
        private StrategyAttribute _classAttribute;

        public static IEnumerable<StrategyInfo> GetStrategiesFromAssembly(Assembly assembly)
        {
            List<StrategyInfo> strategies = new List<StrategyInfo>();
            object a1 = Attribute.GetCustomAttribute(assembly, typeof(StrategySetAttribute)) as StrategySetAttribute;
            if (a1 == null)
                return strategies;
            StrategySetAttribute assemblyAttribute = a1 as StrategySetAttribute;
            foreach (Type t in assembly.GetTypes())
            {
                object a2 = Attribute.GetCustomAttribute(t, typeof(StrategyAttribute)) as StrategyAttribute;
                StrategyAttribute classAttribute = a2 as StrategyAttribute;
                if (a2 == null || !t.GetInterfaces().Any(x => x.AssemblyQualifiedName == typeof(IComputerTurnStrategy).AssemblyQualifiedName))
                    continue;
                StrategyInfo info = new StrategyInfo();
                info._assemblyAttribute = assemblyAttribute;
                info._classAttribute = classAttribute;
                info.Strategy = Activator.CreateInstance(t) as IComputerTurnStrategy;
                strategies.Add(info);
            }
            return strategies;
        }

        public string Title
        {
            get { return _assemblyAttribute.Title; }
        }

        public string Author
        {
            get { return _assemblyAttribute.Author; }
        }

        public string Tag
        {
            get { return _classAttribute.Tag; }
        }

        public DifficultyLevel Level
        {
            get { return _classAttribute.Level; }
        }

        public IComputerTurnStrategy Strategy { private set; get; }
    }
}
