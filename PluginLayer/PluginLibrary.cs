using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

// todo: RL
// - use 1 plugin loader for each interface type request in a dictionary

namespace RLToolkit.Plugin
{
    /// <summary>
    /// Library of plugins. Will allow to load matching assemblies to a specified interface.
    /// </summary>
    public class PluginLibrary
    {
        // Plugin Parameters can be changed before the first call
        public static string pluginPathOverride = null;
        public static bool pluginRecursiveOverride = true;

        // singleton
        private static readonly Lazy<PluginLibrary> instance;

        public static PluginLibrary Instance 
        {
            get 
            { 
                if (instance == null)
                {
                    instance = new Lazy<PluginLibrary>(() => new PluginLibrary(), LazyThreadSafetyMode.ExecutionAndPublication);       
                }
                return instance.Value; 
            }
        }

        private PluginLibrary()
        {
            string pluginPath = AppDomain.CurrentDomain.BaseDirectory;
            if (!string.IsNullOrWhiteSpace(pluginPathOverride))
            {
                pluginPath = pluginPathOverride;
            }
            pluginList = new PluginLoader<IBasePluginContract>(pluginPath, true);
        }

        private PluginLoader<IBasePluginContract> pluginList;

        public List<IBasePluginContract> GetAllPlugins()
        {
            return pluginList.GetPlugins();
        }

        public List<IBasePluginContract> GetPluginByType (Type contractType)
        {
            return GetPluginByType(contractType, -1);
        }

        public List<IBasePluginContract> GetPluginByType (Type contractType, int maxCount)
        {
            List<IBasePluginContract> outputList = new List<IBasePluginContract>();
            foreach (IBasePluginContract plugin in pluginList.GetPlugins())
            {
                if (plugin.GetType().GetInterface(contractType.FullName) != null)
                {
                    outputList.Add(plugin);
                }

                if ((maxCount != -1) && (outputList.Count() >= maxCount))
                {
                    // we have enough, return the list now
                    return outputList;
                }
            }
            return outputList;
        }
    }
}

