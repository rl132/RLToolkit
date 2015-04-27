using System;
using System.Collections.Concurrent;

namespace RLToolkit.Logger
{
    /// <summary>
    /// Extennsion class for the logger
    /// </summary>
	public static class LogExtension
	{
		private static readonly ConcurrentDictionary <string, ILogger> dico = new ConcurrentDictionary<string, ILogger>();

        /// <summary>
        /// Extension method that ease the call of the logger
        /// </summary>
        /// <param name="type">Type of the class</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
		public static ILogger Log<T> (this T type)
		{
			string objectName = typeof(T).FullName;
			return Log(objectName);
		}

        /// <summary>
        /// Extension method that ease the call of the logger
        /// </summary>
        /// <param name="objectName">Object name.</param>
		public static ILogger Log(this string objectName)
		{
            Console.WriteLine("logging for " + objectName);
            return dico.GetOrAdd(objectName, LogManager.loggerFactory.CreateLogger(objectName));
		}
	}
}