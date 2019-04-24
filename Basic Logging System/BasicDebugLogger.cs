using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indieteur.BasicLoggingSystem
{
    public class BasicDebugLogger : BaseLogger
    {
        public enum DebugErrorType
        {
            Error,
            CriticalError
        }

        public BasicDebugLogger(string path, BasicLoggerPathProvided typeOfPathProvided = BasicLoggerPathProvided.Directory, BasicLoggerWriteMode actionIfFileExists = BasicLoggerWriteMode.Overwrite) : base(path, typeOfPathProvided, actionIfFileExists)
        {
        }

        public DebugLogMessageLevel WriteErrorDebugLog(string message, string _namespace, Exception exception, DateTime dateTime, DebugErrorType errorLevel, params string[] additionalMessages)
        {
            return WriteToLog(dateTime, errorLevel.ToBasicLoggerLogLevel(), _namespace, exception, message, additionalMessages);
        }
        public DebugLogMessageLevel WriteErrorDebugLog(string message, string _namespace, Exception exception, DebugErrorType errorLevel, bool includeDateTime = true, params string[] additionalMessages)
        {
            if (includeDateTime)
                return WriteToLog(DateTime.Now, errorLevel.ToBasicLoggerLogLevel(), _namespace, exception, message, additionalMessages);
            else
                return WriteToLog(errorLevel.ToBasicLoggerLogLevel(), _namespace, exception, message, additionalMessages);
        }

        public DebugLogMessageLevel WriteWarningDebugLog(string message, string _namespace, Exception exception, DateTime dateTime, params string[] additionalMessages)
        {
            return WriteToLog(dateTime, BasicLoggerLogLevel.Warning, _namespace, exception, message, additionalMessages);
        }
        public DebugLogMessageLevel WriteWarningDebugLog(string message, string _namespace, Exception exception, bool includeDateTime = true, params string[] additionalMessages)
        {
            if (includeDateTime)
                return WriteToLog(DateTime.Now, BasicLoggerLogLevel.Warning, _namespace, exception, message, additionalMessages);
            else
                return WriteToLog(BasicLoggerLogLevel.Warning, _namespace, exception, message, additionalMessages);
        }

        public DebugLogMessageLevel WriteWarningDebugLog(string message, string _namespace, DateTime dateTime, params string[] additionalMessages)
        {
            return WriteToLog(dateTime, BasicLoggerLogLevel.Warning, _namespace, null, message, additionalMessages);
        }
        public DebugLogMessageLevel WriteWarningDebugLog(string message, string _namespace, bool includeDateTime = true, params string[] additionalMessages)
        {
            if (includeDateTime)
                return WriteToLog(DateTime.Now, BasicLoggerLogLevel.Warning, _namespace, null, message, additionalMessages);
            else
                return WriteToLog(BasicLoggerLogLevel.Warning, _namespace, null, message, additionalMessages);
        }



        public DebugLogMessageLevel WriteInformationDebugLog(string message, string _namespace, DateTime dateTime, params string[] additionalMessages)
        {
            return WriteToLog(dateTime, BasicLoggerLogLevel.Information, _namespace, null, message, additionalMessages);
        }

        public DebugLogMessageLevel WriteInformationDebugLog(string message, string _namespace, bool includeDateTime = true, params string[] additionalMessages)
        {
            if (includeDateTime)
                return WriteToLog(DateTime.Now, BasicLoggerLogLevel.Information, _namespace, null, message, additionalMessages);
            else
                return WriteToLog(BasicLoggerLogLevel.Information, _namespace, null, message, additionalMessages);
        }



        public DebugLogMessageLevel WriteToLog(BasicLoggerLogLevel logLevel, string _namespace, Exception exception, string mainMessage, params string[] additionalMessages)
        {
            DebugLogMessageLevel logMessage = new DebugLogMessageLevel(DateTime.Now, logLevel, mainMessage, _namespace, exception, additionalMessages, false);
            TryWrite(logMessage.ToString());
            return logMessage;
        }

        public DebugLogMessageLevel WriteToLog(DateTime dateTime, BasicLoggerLogLevel logLevel, string _namespace, Exception exception, string mainMessage, params string[] additionalMessages)
        {
            DebugLogMessageLevel logMessage = new DebugLogMessageLevel(dateTime, logLevel, mainMessage, _namespace, exception, additionalMessages);
            TryWrite(logMessage.ToString());
            return logMessage;
        }
    }
}
