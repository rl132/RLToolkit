using System;

using RLToolkit.Plugin;

namespace RLToolkit.Logger
{
    /// <summary>
    /// Interface that allows the usage of different logger
    /// </summary>
	public interface ILogger
	{
        #region Trace
        /// <summary>Log an entry under the trace level</summary>
        /// <param name="message">Message.</param>
        void Trace(string message);

        /// <summary>Log an entry under the trace level</summary>
        /// <param name="message">Message.</param>
        /// <param name="param">Parameter for the string.Format</param>
        void Trace(string message, object[] param);

        /// <summary>Log an entry under the trace level</summary>
        /// <param name="message">Message.</param>
        /// <param name="e">Supply an exception</param>
        void Trace(string message, Exception e);
        #endregion

        #region Debug
        /// <summary>Log an entry under the debug level</summary>
        /// <param name="message">Message.</param>
		void Debug(string message);

        /// <summary>Log an entry under the debug level</summary>
        /// <param name="message">Message.</param>
        /// <param name="param">Parameter for the string.Format</param>
        void Debug(string message, object[] param);

        /// <summary>Log an entry under the debug level</summary>
        /// <param name="message">Message.</param>
        /// <param name="e">Supply an exception</param>
        void Debug(string message, Exception e);
        #endregion

        #region Info
        /// <summary>Log an entry under the info level</summary>
        /// <param name="message">Message.</param>
        void Info(string message);

        /// <summary>Log an entry under the info level</summary>
        /// <param name="message">Message.</param>
        /// <param name="param">Parameter for the string.Format</param>
        void Info(string message, object[] param);

        /// <summary>Log an entry under the debug level</summary>
        /// <param name="message">Message.</param>
        /// <param name="e">Supply an exception</param>
        void Info(string message, Exception e);
        #endregion

        #region Warn
        /// <summary>Log an entry under the warn level</summary>
        /// <param name="message">Message.</param>
        void Warn(string message);

        /// <summary>Log an entry under the warn level</summary>
        /// <param name="message">Message.</param>
        /// <param name="param">Parameter for the string.Format</param>
       void Warn(string message, object[] param);

        /// <summary>Log an entry under the warn level</summary>
        /// <param name="message">Message.</param>
        /// <param name="e">Supply an exception</param>
        void Warn(string message, Exception e);
        #endregion

        #region Fatal
        /// <summary>Log an entry under the fatal level</summary>
        /// <param name="message">Message.</param>
		void Fatal(string message);

        /// <summary>Log an entry under the fatal level</summary>
        /// <param name="message">Message.</param>
        /// <param name="param">Parameter for the string.Format</param>
		void Fatal(string message, object[] param);

        /// <summary>Log an entry under the fatal level</summary>
        /// <param name="message">Message.</param>
        /// <param name="e">Supply an exception</param>
       void Fatal(string message, Exception e);
        #endregion
	}
}