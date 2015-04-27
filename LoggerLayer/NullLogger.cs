using System;

namespace RLToolkit.Logger
{
    /// <summary>
    /// Null logger implementing  the ILogger interface. Note: this logger does nothing.
    /// </summary>
	public class NullLogger : ILogger
	{
        #region Logger-Trace
        /// <summary>
        /// Log an entry under the trace level
        /// </summary>
        /// <param name="message">Message.</param>
        public void Trace (string message)
        {
            // do nothing
        }

        /// <summary>
        /// Log an entry under the trace level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="param">Parameter for the string.Format</param>
        public void Trace (string message, object[] param)
        {
            // do nothing
        }

        /// <summary>
        /// Log an entry under the trace level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="e">E.</param>
        public void Trace (string message, Exception e)
        {
            // do nothing
        }
        #endregion

        /// <summary>
        /// Log an entry under the debug level
        /// </summary>
        /// <param name="message">Message.</param>
        public void Debug (string message)
		{
			// do nothing
		}

        /// <summary>
        /// Log an entry under the debug level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="param">Parameter for the string.Format</param>
        public void Debug (string message, object[] param)
        {
            // do nothing
        }

        /// <summary>
        /// Log an entry under the debug level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="e">E.</param>
        public void Debug (string message, Exception e)
        {
            // do nothing
        }

        /// <summary>
        /// Log an entry under the info level
        /// </summary>
        /// <param name="message">Message.</param>
		public void Info (string message)
		{
			// do nothing
		}

        /// <summary>
        /// Log an entry under the info level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="param">Parameter for the string.Format</param>
        public void Info (string message, object[] param)
		{
			// do nothing
		}

        /// <summary>
        /// Log an entry under the info level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="e">E.</param>
        public void Info (string message, Exception e)
        {
            // do nothing
        }

        /// <summary>
        /// Log an entry under the warn level
        /// </summary>
        /// <param name="message">Message.</param>
		public void Warn (string message)
		{
			// do nothing
		}

        /// <summary>
        /// Log an entry under the warn level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="param">Parameter for the string.Format</param>
		public void Warn (string message, object[] param)
		{
			// do nothing
		}

        /// <summary>
        /// Log an entry under the warn level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="e">E.</param>
        public void Warn (string message, Exception e)
        {
            // do nothing
        }

        /// <summary>
        /// Log an entry under the fatal level
        /// </summary>
        /// <param name="message">Message.</param>
		public void Fatal (string message)
		{
			// do nothing
		}

        /// <summary>
        /// Log an entry under the fatal level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="param">Parameter for the string.Format</param>
		public void Fatal (string message, object[] param)
		{
			// do nothing
		}

        /// <summary>
        /// Log an entry under the fatal level
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="e">E.</param>
        public void Fatal (string message, Exception e)
        {
            // do nothing
        }
	}
}