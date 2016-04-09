using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunch.Logging
{
    public class Logger
    {
        private ILog _log = null;
        internal Logger(ILog log)
        {
            _log = log;
        }



        public static void Configure()
        {
            XmlConfigurator.Configure();
        }
        public static Logger For(object LoggedObject)
        {
            if (LoggedObject != null)
                return For(LoggedObject.GetType());
            else
                return For(null);
        }
        public static Logger For(Type ObjectType)
        {
            if (ObjectType != null)
                return new Logger(LogManager.GetLogger(ObjectType.Name));
            else
                return new Logger(LogManager.GetLogger(string.Empty));
        }



        public void Debug(object message)
        {
            this._log.Debug(message);
        }
        public void Debug(object message, Exception exception)
        {
            this._log.Debug(message, exception);
        }


        public void Error(object message)
        {
            this._log.Error(message);
        }
        public void Error(object message, Exception exception)
        {
            this._log.Error(message, exception);
        }


        public void Fatal(object message)
        {
            this._log.Fatal(message);
        }
        public void Fatal(object message, Exception exception)
        {
            this._log.Fatal(message, exception);
        }


        public void Info(object message)
        {
            this._log.Info(message);
        }
        public void Info(object message, Exception exception)
        {
            this._log.Info(message, exception);
        }


        public void Warn(object message)
        {
            this._log.Warn(message);
        }
        public void Warn(object message, Exception exception)
        {
            this._log.Warn(message, exception);
        }
    }
}
