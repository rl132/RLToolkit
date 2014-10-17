using System;
using System.IO;
using System.Reflection;

using log4net;
using log4net.Config;

namespace RLToolkit
{
	public class Log4NetLayer : ILogger
	{
        #region Variables
		private readonly ILog log;
        #endregion

        #region Init
		static Log4NetLayer ()
		{
			LogManager.Instance.Log().Info("Logging system ready.");
		}

		public Log4NetLayer (ILog logIn)
		{
			log = logIn;
		}
        #endregion

        #region Logger-Debug
		public void Debug (string message)
		{
			if (log.IsDebugEnabled && message != null) {
				log.Debug(message);
			}
		}

		public void Debug (string message, object[] param)
		{
			if (log.IsDebugEnabled && message != null) {
				log.Debug(string.Format (message, param));
			}
		}

        public void Debug (string message, Exception e)
        {
            if (log.IsDebugEnabled && message != null && e != null) {
                log.Debug("Exception thrown: " + message + Environment.NewLine + e.ToString());
            }
        }
        #endregion

        #region Logger-Info
		public void Info (string message)
		{
			if (log.IsInfoEnabled && message != null) {
				log.Info(message);
			}
		}

        public void Info (string message, object[] param)
		{
			if (log.IsInfoEnabled && message != null) {
				log.Info(string.Format (message, param));
			}
		}

        public void Info (string message, Exception e)
        {
            if (log.IsInfoEnabled && message != null && e != null) {
                log.Info("Exception thrown: " + message + Environment.NewLine + e.ToString());
            }
        }
        #endregion

        #region Logger-Warn
		public void Warn (string message)
		{
			if (log.IsWarnEnabled && message != null) {
				log.Warn(message);
			}
		}

		public void Warn (string message, object[] param)
		{
			if (log.IsWarnEnabled && message != null) {
				log.Warn(string.Format (message, param));
			}
		}

        public void Warn (string message, Exception e)
        {
            if (log.IsWarnEnabled && message != null && e != null) {
                log.Warn("Exception thrown: " + message + Environment.NewLine + e.ToString());
            }
        }
        #endregion

        #region Logger-Fatal
		public void Fatal (string message)
		{
			if (log.IsFatalEnabled && message != null) {
				log.Fatal(message);
			}
		}

		public void Fatal (string message, object[] param)
		{
			if (log.IsFatalEnabled && message != null) {
				log.Fatal(string.Format (message, param));
			}
		}

        public void Fatal (string message, Exception e)
        {
            if (log.IsFatalEnabled && message != null && e != null) {
                log.Fatal("Exception thrown: " + message + Environment.NewLine + e.ToString());
            }
        }
        #endregion

	}
}
