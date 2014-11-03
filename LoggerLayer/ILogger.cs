using System;

namespace RLToolkit
{
    /// <summary>
    /// Interface that allows the usage of different logger (not yet implemented)
    /// </summary>
	public interface ILogger
	{
		// TODO: Add more way to implement the logger interface. 

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