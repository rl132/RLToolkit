using System;
using System.Threading;
using System.IO;
using log4net;
using log4net.Config;

namespace RLToolkit.Logger
{
    /// <summary>
    /// Helper class that handles the logger
    /// </summary>
	public class LogManager
	{
        /// <summary>The default config file.</summary>
		public const string defaultConfigFile = @"logger.config";
		private static readonly Lazy<LogManager> _managerInstance = new Lazy<LogManager>(() => new LogManager(), LazyThreadSafetyMode.ExecutionAndPublication);       

        /// <summary>Gets the instance of the logger</summary>
        /// <value>The instance.</value>
		public static LogManager Instance 
        {
			get { return _managerInstance.Value; }
		}

		private LogManager ()
		{
			string filePath = Path.Combine (AppDomain.CurrentDomain.BaseDirectory, defaultConfigFile);
			if (File.Exists (filePath)) {
				Console.WriteLine("Using the following configuration file for Log4Net:\n" + filePath);
				XmlConfigurator.ConfigureAndWatch (new FileInfo (filePath));
			} else {
				// file not found!
				Console.WriteLine("Configuration file for Log4Net not found:\n" + filePath);
                XmlConfigurator.Configure();
			}
		}
	}
}