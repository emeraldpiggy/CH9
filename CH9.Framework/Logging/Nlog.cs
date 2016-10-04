using System;
using NLog;

namespace CH9.Framework.Logging
{
    internal class Nlog : ILog
    {
        private readonly Logger _logger;

        public Nlog()
        {
          _logger = LogManager.GetCurrentClassLogger();  
        }

        public Nlog(Type type)
        {
            _logger = LogManager.GetLogger(type.FullName, type);
        }

        public void Log(LogLevel level, string format, params object[] args)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _logger.Debug(format, args);
                    break;
                default:
                    _logger.Info(format, args);
                    break;
            }
        }
    }
}