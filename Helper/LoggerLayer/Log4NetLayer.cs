using System;
using System.IO;
using System.Reflection;

using log4net;
using log4net.Config;

namespace RLToolkit
{
	public class Log4NetLayer : ILogger
	{
		private readonly ILog log;

		static Log4NetLayer ()
		{
			LogManager.Instance.Log().Info("Logging system ready.");
		}

		public Log4NetLayer (ILog logIn)
		{
			log = logIn;
		}

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
	}
}

