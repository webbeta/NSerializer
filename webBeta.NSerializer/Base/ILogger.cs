using System;

namespace webBeta.NSerializer.Base
{
    public interface ILogger
    {
        public enum Level {
            ERROR
        }

        Level GetLevel();
        void SetLevel(Level level);

        void Error(string message);
    }
}