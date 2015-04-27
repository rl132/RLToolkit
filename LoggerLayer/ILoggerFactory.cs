using System;

using RLToolkit.Plugin;

namespace RLToolkit.Logger
{
    /// <summary>
    /// Logger Factory Interface. Plugin-compatible
    /// </summary>
    public interface ILoggerFactory : IBasePluginContract
    {
        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <returns>The returning logger.</returns>
        /// <param loggerName="name">Name of the logger</param>
        ILogger CreateLogger(string loggerName);
    }
}

