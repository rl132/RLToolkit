using System;

namespace RLToolkit.Logger
{
    public interface ILogSystem : ILogger
    {
        void Initialize(string loggerName);

    }
}

