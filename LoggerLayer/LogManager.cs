using System;
using System.Threading;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

using RLToolkit.Plugin;

namespace RLToolkit.Logger
{
    /// <summary>
    /// Helper class that handles the logger
    /// </summary>
	public class LogManager
	{
        /// <summary>The default config file.</summary>
        private static readonly Lazy<LogManager> _managerInstance = new Lazy<LogManager>(() => new LogManager(), LazyThreadSafetyMode.ExecutionAndPublication);       

        /// <summary>The factory to use to create the desired logger</summary>
        public static ILoggerFactory loggerFactory;

        /// <summary>Gets the instance of the log manager</summary>
        /// <value>The instance.</value>
		public static LogManager Instance 
        {
			get { return _managerInstance.Value; }
		}

		private LogManager ()
		{
			// try to get the logging plugins.
            IEnumerable<ILoggerFactory> rawPlugins = PluginLibrary.Instance.GetPluginByType(typeof(ILoggerFactory), -1).OfType<ILoggerFactory>();

            List<ILoggerFactory> plugins = new List<ILoggerFactory>();
            foreach (ILoggerFactory f in rawPlugins)
            {
                if (f.GetType() != typeof(NullLoggerFactory))
                {
                    plugins.Add(f);
                }
            }

            if (plugins.Count() == 0)
            {
                // Null logger
                Console.WriteLine("ERROR: NO LOGGER FOUND, USING NULL LOGGER");
                loggerFactory = new NullLoggerFactory();
                return;
            } else if (plugins.Count() > 1)
            {
                Console.WriteLine("ERROR: MORE THAN ONE LOGGER FOUND, USING THE FIRST ONE (" + plugins.ElementAt(0).GetType().Name + ")");
            }

            Console.WriteLine("Info: using logger " + plugins.ElementAt(0).GetType().Name);
            // fetch the info from the plugin
            loggerFactory = plugins.ElementAt(0);
		}
	}
}