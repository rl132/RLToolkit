using System;

namespace RLToolkit
{
	public class NullLogger : ILogger
	{
		// RL: no longer used. will be when we migrate to
		// the new logging system that will have interchangeable 
		// logger implementation.
		public void Debug (string message)
		{
			// do nothing
		}

        public void Debug (string message, object[] param)
        {
            // do nothing
        }

        public void Debug (string message, Exception e)
        {
            // do nothing
        }

		public void Info (string message)
		{
			// do nothing
		}

        public void Info (string message, object[] param)
		{
			// do nothing
		}

        public void Info (string message, Exception e)
        {
            // do nothing
        }

		public void Warn (string message)
		{
			// do nothing
		}

		public void Warn (string message, object[] param)
		{
			// do nothing
		}

        public void Warn (string message, Exception e)
        {
            // do nothing
        }

		public void Fatal (string message)
		{
			// do nothing
		}

		public void Fatal (string message, object[] param)
		{
			// do nothing
		}

        public void Fatal (string message, Exception e)
        {
            // do nothing
        }
	}
}