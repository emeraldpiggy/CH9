using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CH9.MVVM
{
    internal class CaliburnLog:Caliburn.Micro.ILog
    {
        public void Info(string format, params object[] args)
        {
            this.log
        }

        public void Warn(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Error(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
