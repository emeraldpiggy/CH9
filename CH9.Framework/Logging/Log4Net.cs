using System;

namespace CH9.Framework.Logging
{
    internal class Log4Net : ILog
    {
        public Log4Net(Type type)
        {
           
        }

        public void Log(LogLevel level, string format, params object[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}