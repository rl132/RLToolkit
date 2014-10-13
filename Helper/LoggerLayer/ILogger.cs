using System;

namespace RLToolkit
{
	public interface ILogger
	{
		// TODO: Add more way to implement the logger interface. 
		// -add exception logging

		void Debug(string message);
		void Debug(string message, object[] param);

		void Info(string message);
		void Info(string message, object[] param);

		void Warn(string message);
		void Warn(string message, object[] param);

		void Fatal(string message);
		void Fatal(string message, object[] param);
	}
}