using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indieteur.BasicLoggingSystem
{
    public class BasicLogCombo : IDisposable
    {
        public BasicUserLogger UserLog { get; private set; }
        public BasicDebugLogger DebugLog { get; private set; }

        bool disposed = false;

        public BasicLogCombo(string pathToUserLogFileOrFolder, string pathToDebugLogFileOrFolder, BasicLoggerPathProvided typeOfPathProvided = BasicLoggerPathProvided.Directory, BasicLoggerWriteMode actionIfFileExists = BasicLoggerWriteMode.Overwrite)
        {
            if (string.IsNullOrWhiteSpace(pathToUserLogFileOrFolder) )
                throw new ArgumentNullException("pathToUserLog argument is either null, whitespace or empty.");

            if (string.IsNullOrWhiteSpace(pathToDebugLogFileOrFolder))
                throw new ArgumentNullException("pathToDebugLog argument is either null, whitespace or empty.");

            if (typeOfPathProvided == BasicLoggerPathProvided.Directory)
            {
                string fileName = DateTime.Now.ToFileNameFormat();
                DirectoryInfo dInfo = new DirectoryInfo(pathToUserLogFileOrFolder);
                pathToUserLogFileOrFolder = dInfo.FullName + "\\" + fileName;

                dInfo = new DirectoryInfo(pathToDebugLogFileOrFolder);
                pathToDebugLogFileOrFolder = dInfo.FullName + "\\" + fileName;
            }
            StartLogging(pathToUserLogFileOrFolder, pathToDebugLogFileOrFolder, actionIfFileExists);
        }

        public void ChangeLogFile(string pathToUserLog, string pathToDebugLog, BasicLoggerWriteMode actionIfFileExists)
        {
            StopLogging();
            StartLogging(pathToUserLog, pathToDebugLog, actionIfFileExists);
        }

        void StartLogging(string userLogPath, string debugLogPath, BasicLoggerWriteMode actionIfFileExists)
        {
            UserLog = new BasicUserLogger(userLogPath, BasicLoggerPathProvided.File, actionIfFileExists);
            DebugLog = new BasicDebugLogger(debugLogPath, BasicLoggerPathProvided.File, actionIfFileExists);

        }

        public void WriteErrorLog(string message, string _namespace, Exception exception, DateTime dateTime, BasicDebugLogger.DebugErrorType errorLevel, params string[] additionalMessages)
        {
            LogMessageWLevel userLog;
            DebugLogMessageLevel debugLog;
            WriteErrorLog(message, _namespace, exception, dateTime, errorLevel, out userLog, out debugLog, additionalMessages);
        }

        public void WriteErrorLog(string message, string _namespace, Exception exception, DateTime dateTime, BasicDebugLogger.DebugErrorType errorLevel, out LogMessageWLevel userLog, out DebugLogMessageLevel debugLog, params string[] additionalMessages)
        {
            userLog = null;
            debugLog = null;
            List<Exception> exceptions = new List<Exception>();
            if (UserLog == null)
                exceptions.Add(new NoUserLogException("The User Log is set to null."));
            else
            {
                try
                {
                    userLog = UserLog.WriteToLog(message, dateTime, errorLevel.ToBasicLoggerLogLevel() );
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }


            if (DebugLog == null)
                exceptions.Add(new NoDebugLogException("The Debug Log is set to null."));
            else
            {
                try
                {
                    debugLog = DebugLog.WriteErrorDebugLog(message, _namespace, exception, dateTime, errorLevel, additionalMessages);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Count > 0)
                throw new MultipleExceptions("An error or multiple errors have occured while trying to write to log files.", exceptions.ToArray());
        }
        public void WriteErrorLog(string message, string _namespace, Exception exception, BasicDebugLogger.DebugErrorType errorLevel, bool includeDateTime = true, params string[] additionalMessages)
        {
            LogMessageWLevel userLog;
            DebugLogMessageLevel debugLog;
            WriteErrorLog(message, _namespace, exception, errorLevel, out userLog, out debugLog, includeDateTime, additionalMessages);
        }
        public void WriteErrorLog(string message, string _namespace, Exception exception, BasicDebugLogger.DebugErrorType errorLevel, out LogMessageWLevel userLog, out DebugLogMessageLevel debugLog, bool includeDateTime = true, params string[] additionalMessages)
        {
            userLog = null;
            debugLog = null;
            List<Exception> exceptions = new List<Exception>();
            if (UserLog == null)
                exceptions.Add(new NoUserLogException("The User Log is set to null."));
            else
            {
                try
                {
                    userLog = UserLog.WriteToLog(message, errorLevel.ToBasicLoggerLogLevel(), includeDateTime);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }


            if (DebugLog == null)
                exceptions.Add(new NoDebugLogException("The Debug Log is set to null."));
            else
            {
                try
                {
                    debugLog = DebugLog.WriteErrorDebugLog(message, _namespace, exception, errorLevel, includeDateTime, additionalMessages);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Count > 0)
                throw new MultipleExceptions("An error or multiple errors have occured while trying to write to log files.", exceptions.ToArray());
        }


        public void WriteWarningLog(string message, string _namespace, Exception exception, DateTime dateTime, params string[] additionalMessages)
        {
            LogMessageWLevel userLog;
            DebugLogMessageLevel debugLog;
            WriteWarningLog(message, _namespace, exception, dateTime, out userLog, out debugLog, additionalMessages);
        }

        public void WriteWarningLog(string message, string _namespace, Exception exception, DateTime dateTime, out LogMessageWLevel userLog, out DebugLogMessageLevel debugLog, params string[] additionalMessages)
        {
            userLog = null;
            debugLog = null;
            List<Exception> exceptions = new List<Exception>();
            if (UserLog == null)
                exceptions.Add(new NoUserLogException("The User Log is set to null."));
            else
            {
                try
                {
                    userLog = UserLog.WriteToLog(message, dateTime, BasicLoggerLogLevel.Warning);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }


            if (DebugLog == null)
                exceptions.Add(new NoDebugLogException("The Debug Log is set to null."));
            else
            {
                try
                {
                    debugLog = DebugLog.WriteWarningDebugLog(message, _namespace, exception, dateTime, additionalMessages);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Count > 0)
                throw new MultipleExceptions("An error or multiple errors have occured while trying to write to log files.", exceptions.ToArray());
        }

        public void WriteWarningLog(string message, string _namespace, Exception exception, bool includeDateTime = true, params string[] additionalMessages)
        {
            LogMessageWLevel userLog;
            DebugLogMessageLevel debugLog;
            WriteWarningLog(message, _namespace, exception, out userLog, out debugLog, includeDateTime, additionalMessages);
        }

        public void WriteWarningLog(string message, string _namespace, Exception exception, out LogMessageWLevel userLog, out DebugLogMessageLevel debugLog, bool includeDateTime = true, params string[] additionalMessages)
        {
            userLog = null;
            debugLog = null;
            List<Exception> exceptions = new List<Exception>();
            if (UserLog == null)
                exceptions.Add(new NoUserLogException("The User Log is set to null."));
            else
            {
                try
                {
                    userLog = UserLog.WriteToLog(message, BasicLoggerLogLevel.Warning, includeDateTime);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }


            if (DebugLog == null)
                exceptions.Add(new NoDebugLogException("The Debug Log is set to null."));
            else
            {
                try
                {
                    debugLog = DebugLog.WriteWarningDebugLog(message, _namespace, exception, includeDateTime, additionalMessages);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Count > 0)
                throw new MultipleExceptions("An error or multiple errors have occured while trying to write to log files.", exceptions.ToArray());
        }
        public void WriteWarningLog(string message, string _namespace, DateTime dateTime, params string[] additionalMessages)
        {
            LogMessageWLevel userLog;
            DebugLogMessageLevel debugLog;
            WriteWarningLog(message, _namespace, dateTime, out userLog, out debugLog, additionalMessages);
        }
        public void WriteWarningLog(string message, string _namespace, DateTime dateTime, out LogMessageWLevel userLog, out DebugLogMessageLevel debugLog, params string[] additionalMessages)
        {
            userLog = null;
            debugLog = null;
            List<Exception> exceptions = new List<Exception>();
            if (UserLog == null)
                exceptions.Add(new NoUserLogException("The User Log is set to null."));
            else
            {
                try
                {
                    userLog = UserLog.WriteToLog(message, dateTime, BasicLoggerLogLevel.Warning);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }


            if (DebugLog == null)
                exceptions.Add(new NoDebugLogException("The Debug Log is set to null."));
            else
            {
                try
                {
                    debugLog = DebugLog.WriteWarningDebugLog(message, _namespace, dateTime, additionalMessages);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Count > 0)
                throw new MultipleExceptions("An error or multiple errors have occured while trying to write to log files.", exceptions.ToArray());
        }

        public void WriteWarningLog(string message, string _namespace, bool includeDateTime = true, params string[] additionalMessages)
        {
            LogMessageWLevel userLog;
            DebugLogMessageLevel debugLog;
            WriteWarningLog(message, _namespace, out userLog, out debugLog, includeDateTime, additionalMessages);
        }

        public void WriteWarningLog(string message, string _namespace, out LogMessageWLevel userLog, out DebugLogMessageLevel debugLog, bool includeDateTime = true, params string[] additionalMessages)
        {
            userLog = null;
            debugLog = null;
            List<Exception> exceptions = new List<Exception>();
            if (UserLog == null)
                exceptions.Add(new NoUserLogException("The User Log is set to null."));
            else
            {
                try
                {
                    userLog = UserLog.WriteToLog(message, BasicLoggerLogLevel.Warning, includeDateTime);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }


            if (DebugLog == null)
                exceptions.Add(new NoDebugLogException("The Debug Log is set to null."));
            else
            {
                try
                {
                    debugLog = DebugLog.WriteWarningDebugLog(message, _namespace, includeDateTime, additionalMessages);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Count > 0)
                throw new MultipleExceptions("An error or multiple errors have occured while trying to write to log files.", exceptions.ToArray());
        }

        public void WriteInformationLog(string message, string _namespace, DateTime dateTime, params string[] additionalMessages)
        {
            LogMessageWLevel userLog;
            DebugLogMessageLevel debugLog;
            WriteInformationLog(message, _namespace, dateTime, out userLog, out debugLog, additionalMessages);

        }
        public void WriteInformationLog(string message, string _namespace, DateTime dateTime, out LogMessageWLevel userLog, out DebugLogMessageLevel debugLog, params string[] additionalMessages)
        {
            userLog = null;
            debugLog = null;
            List<Exception> exceptions = new List<Exception>();
            if (UserLog == null)
                exceptions.Add(new NoUserLogException("The User Log is set to null."));
            else
            {
                try
                {
                    userLog = UserLog.WriteToLog(message, dateTime, BasicLoggerLogLevel.Information);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }


            if (DebugLog == null)
                exceptions.Add(new NoDebugLogException("The Debug Log is set to null."));
            else
            {
                try
                {
                    debugLog = DebugLog.WriteInformationDebugLog(message, _namespace, dateTime, additionalMessages);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Count > 0)
                throw new MultipleExceptions("An error or multiple errors have occured while trying to write to log files.", exceptions.ToArray());
        }

        public void WriteInformationLog(string message, string _namespace, bool includeDateTime = true, params string[] additionalMessages)
        {
            LogMessageWLevel userLog;
            DebugLogMessageLevel debugLog;
            WriteInformationLog(message, _namespace, out userLog, out debugLog, includeDateTime, additionalMessages);
        }
        public void WriteInformationLog(string message, string _namespace, out LogMessageWLevel userLog, out DebugLogMessageLevel debugLog, bool includeDateTime = true, params string[] additionalMessages)
        {
            userLog = null;
            debugLog = null;
            List<Exception> exceptions = new List<Exception>();
            if (UserLog == null)
                exceptions.Add(new NoUserLogException("The User Log is set to null."));
            else
            {
                try
                {
                    userLog = UserLog.WriteToLog(message, BasicLoggerLogLevel.Information, includeDateTime);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }


            if (DebugLog == null)
                exceptions.Add(new NoDebugLogException("The Debug Log is set to null."));
            else
            {
                try
                {
                    debugLog = DebugLog.WriteInformationDebugLog(message, _namespace, includeDateTime, additionalMessages);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
                throw new MultipleExceptions("An error or multiple errors have occured while trying to write to log files.", exceptions.ToArray());
        }


        public LogMessageWLevel WriteSimpleMessageToLogs(string message, BasicLoggerLogLevel logLevel, bool includeDateTime = true)
        {

            LogMessageWLevel logMessage = new LogMessageWLevel(DateTime.Now, logLevel, message, includeDateTime);
            TryWriteBoth(logMessage.ToString());
            return logMessage;
        }
        public LogMessageWLevel WriteSimpleMessageToLogs(string message, DateTime dateTime, BasicLoggerLogLevel logLevel)
        {
            LogMessageWLevel logMessage = new LogMessageWLevel(dateTime, logLevel, message);
            TryWriteBoth(logMessage.ToString());
            return logMessage;
        }

        public LogMessage WriteSimpleMessageToLogs(string message, DateTime dateTime)
        {
            LogMessage logMessage = new LogMessage(dateTime, message);
            TryWriteBoth(logMessage.ToString());
            return logMessage;
        }

        public void WriteEmptyLineToBothLogs()
        {
            TryWriteBoth(null);
        }

        public LogMessage WriteSimpleMessageToLogs(string message, bool includeDateTime = true)
        {
            LogMessage logMessage = new LogMessage(DateTime.Now, message, includeDateTime);
            TryWriteBoth(logMessage.ToString());
            return logMessage;

        }

        public void StopLogging()
        {
            if (UserLog != null)
            {
                UserLog.StopLogging();
                UserLog = null;
            }
            if (DebugLog != null)
            {
                DebugLog.StopLogging();
                DebugLog = null;
            }
        }


        protected void TryWriteBoth(string message, bool asLine = true)
        {
            List<Exception> exceptions = new List<Exception>();
            if (UserLog == null)
                exceptions.Add(new NoUserLogException("The User Log is set to null."));
            else
            {
                try
                {
                    UserLog.TryWrite(message, asLine);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (DebugLog == null)
                exceptions.Add(new NoDebugLogException("The Debug Log is set to null."));
            else
            {
                try
                {
                    DebugLog.TryWrite(message, asLine);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Count > 0)
                throw new MultipleExceptions("An error or multiple errors occured while trying to write to log files.", exceptions.ToArray());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Stop Logging and close the stream writer.
                    StopLogging();
                }
                // Note disposing has been done.
                disposed = true;

            }
        }
    }
}
