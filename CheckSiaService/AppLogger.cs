using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace CheckSiaService
{
    public class AppLogger
    {
        private Logger logger = null;
        public AppLogger(string clsName)
        {
            logger = LogManager.GetLogger(clsName);
            clsName = null;
        }

        public void Info(string message)
        {
            logger.Info(message);
            //FileLogger.WriteLog(message);
            message = null;
        }

        public void Debug(string message)
        {
            logger.Debug(message);
            message = null;
        }
        public void Error(Exception ex, string message)
        {
            string trace = ex.ToString();
            logger.Error(message + trace);

            //FileLogger.WriteLog(message + trace);
            message = null;
            trace = null;
        }
    }
}
