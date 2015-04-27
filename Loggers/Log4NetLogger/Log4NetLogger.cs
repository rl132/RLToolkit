using System;
using System.IO;
using System.Reflection;

using log4net;
using log4net.Config;

namespace RLToolkit.Logger
{
    /// <summary>
    /// Log4Net implementing  the ILogger interface. 
    /// </summary>
    public class Log4NetLogger : ILogger
	{
        #region Variables
		private readonly ILog log;
        #endregion

        #region Constants
        /// <summary>The default config file for log4net.</summary>
        public const string defaultConfigFile = @"logger.config";
        #endregion

        #region Init
        /// <summary>Report that the system if functional on firat usage</summary>
		static Log4NetLogger ()
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

			LogManager.Instance.Log().Info("Logging system ready.");
		}

        /// <summary>
        /// Constructor that initialiuze the internal logger
        /// </summary>
        /// <param name="logIn">Logger input</param>
		public Log4NetLogger (ILog logIn)
		{
			log = logIn;
		}
        #endregion

        #region Logger-Trace
        /// <summary>
        /// Log an entry under the trace level
        /// </summary>
        /// <param name="message">Message.</param>
        public void Trace (string message)
        {

        }

        /// <summary>
        /// Log an entry under the trace level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="param">Parameter for the string.Format</param>
        public void Trace (string message, object[] param)
        {

        }

        /// <summary>
        /// Log an entry under the trace level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="e">E.</param>
        public void Trace (string message, Exception e)
        {

        }
        #endregion


        #region Logger-Debug
        /// <summary>
        /// Log an entry under the debug level
        /// </summary>
        /// <param name="message">Message.</param>
		public void Debug (string message)
		{
			if (log.IsDebugEnabled && message != null) {
				log.Debug(message);
			}
		}

        /// <summary>
        /// Log an entry under the debug level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="param">Parameter for the string.Format</param>
		public void Debug (string message, object[] param)
		{
			if (log.IsDebugEnabled && message != null) {
				log.Debug(string.Format (message, param));
			}
		}

        /// <summary>
        /// Log an entry under the debug level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="e">E.</param>
        public void Debug (string message, Exception e)
        {
            if (log.IsDebugEnabled && message != null && e != null) {
                log.Debug("Exception thrown: " + message + Environment.NewLine + e.ToString());
            }
        }
        #endregion

        #region Logger-Info
        /// <summary>
        /// Log an entry under the info level
        /// </summary>
        /// <param name="message">Message.</param>
		public void Info (string message)
		{
			if (log.IsInfoEnabled && message != null) {
				log.Info(message);
			}
		}

        /// <summary>
        /// Log an entry under the info level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="param">Parameter for the string.Format</param>
        public void Info (string message, object[] param)
		{
			if (log.IsInfoEnabled && message != null) {
				log.Info(string.Format (message, param));
			}
		}

        /// <summary>
        /// Log an entry under the info level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="e">E.</param>
        public void Info (string message, Exception e)
        {
            if (log.IsInfoEnabled && message != null && e != null) {
                log.Info("Exception thrown: " + message + Environment.NewLine + e.ToString());
            }
        }
        #endregion

        #region Logger-Warn
        /// <summary>
        /// Log an entry under the warn level
        /// </summary>
        /// <param name="message">Message.</param>
		public void Warn (string message)
		{
			if (log.IsWarnEnabled && message != null) {
				log.Warn(message);
			}
		}

        /// <summary>
        /// Log an entry under the warn level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="param">Parameter for the string.Format</param>
		public void Warn (string message, object[] param)
		{
			if (log.IsWarnEnabled && message != null) {
				log.Warn(string.Format (message, param));
			}
		}

        /// <summary>
        /// Log an entry under the warn level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="e">E.</param>
        public void Warn (string message, Exception e)
        {
            if (log.IsWarnEnabled && message != null && e != null) {
                log.Warn("Exception thrown: " + message + Environment.NewLine + e.ToString());
            }
        }
        #endregion

        #region Logger-Fatal
        /// <summary>
        /// Log an entry under the fatal level
        /// </summary>
        /// <param name="message">Message.</param>
		public void Fatal (string message)
		{
			if (log.IsFatalEnabled && message != null) {
				log.Fatal(message);
			}
		}

        /// <summary>
        /// Log an entry under the fatal level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="param">Parameter for the string.Format</param>
		public void Fatal (string message, object[] param)
		{
			if (log.IsFatalEnabled && message != null) {
				log.Fatal(string.Format (message, param));
			}
		}

        /// <summary>
        /// Log an entry under the fatal level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="e">E.</param>
        public void Fatal (string message, Exception e)
        {
            if (log.IsFatalEnabled && message != null && e != null) {
                log.Fatal("Exception thrown: " + message + Environment.NewLine + e.ToString());
            }
        }
        #endregion
	}
}