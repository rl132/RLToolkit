using System;

namespace RLToolkit.Logger
{
    /// <summary>
    /// Null Logger Factory. Default logger for the toolkit.
    /// </summary>
    public class NullLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <returns>The null logger.</returns>
        /// <param name="name">Name of the logger</param>
        public ILogger CreateLogger(string name)
        {
            return new NullLogger();
        }
    }
}