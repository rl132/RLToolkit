using System;
using System.Collections.Concurrent;

using log4net;

namespace RLToolkit
{
	public static class LogExtension
	{
		private static readonly ConcurrentDictionary <string, ILogger> dico = new ConcurrentDictionary<string, ILogger>();

		public static ILogger Log<T> (this T type)
		{
			string objectName = typeof(T).FullName;
			return Log(objectName);
		}

		public static ILogger Log(this string objectName)
		{
			return dico.GetOrAdd(objectName, new Log4NetLayer(log4net.LogManager.GetLogger(objectName)));
		}
	}
}

