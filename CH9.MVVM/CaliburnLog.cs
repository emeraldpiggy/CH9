using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CH9.Framework.Logging;

namespace CH9.MVVM
{
    internal class CaliburnLog:Caliburn.Micro.ILog
    {
        public void Info(string format, params object[] args)
        {
            this.Log(LogLevel.Info, format, args);
        }

        public void Warn(string format, params object[] args)
        {
            this.Log(LogLevel.Debug, format, args);
        }

        public void Error(Exception exception)
        {
            this.Log(LogLevel.Debug, "{0} {1}", exception.Message, exception.InnerException);
        }
    }
}
