using System;

namespace RLToolkit
{
	public interface ILogger
	{
		// TODO: Add more way to implement the logger interface. 

		void Debug(string message);
        void Debug(string message, object[] param);
        void Debug(string message, Exception e);

		void Info(string message);
		void Info(string message, object[] param);
        void Info(string message, Exception e);

		void Warn(string message);
		void Warn(string message, object[] param);
        void Warn(string message, Exception e);

		void Fatal(string message);
		void Fatal(string message, object[] param);
        void Fatal(string message, Exception e);
	}
}