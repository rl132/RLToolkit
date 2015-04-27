using System;

using log4net;

namespace RLToolkit.Logger
{
    /// <summary>
    /// Log4net logger factory.
    /// </summary>
    public class Log4NetLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <returns>The Log4Net logger.</returns>
        /// <param name="name">Name of the logger</param>
        public ILogger CreateLogger(string name)
        {
            return new Log4NetLogger(log4net.LogManager.GetLogger(name));
        }
    }
}

