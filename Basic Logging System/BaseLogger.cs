using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Indieteur.BasicLoggingSystem
{
    public enum BasicLoggerWriteMode
    {
        Append,
        Overwrite,
        ThrowError
    }

    public enum BasicLoggerPathProvided
    {
        /// <summary>
        /// Provided argument as path is directory.
        /// </summary>
        Directory,
        /// <summary>
        /// Provided argument as path is file.
        /// </summary>
        File
    }

    public enum BasicLoggerLogLevel
    {
        Information,
        Warning,
        Error,
        CriticalError
    }

    public abstract class BaseLogger : IDisposable
    {

        /// <summary>
        /// Gets or sets the path to the log file.
        /// </summary>
        public virtual string LogFilePath { get; private set; }

        bool disposed = false;

        StreamWriter _openFile;



        public BaseLogger(string path, BasicLoggerPathProvided typeOfPathProvided = BasicLoggerPathProvided.Directory, BasicLoggerWriteMode actionIfFileExists = BasicLoggerWriteMode.Overwrite)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("Path argument is either null, whitespace or empty.");

            if (typeOfPathProvided == BasicLoggerPathProvided.Directory)
            {
                DirectoryInfo dInfo = new DirectoryInfo(path);
                path = dInfo.FullName + "\\" + DateTime.Now.ToFileNameFormat();
            }
            BeginLogging(path, actionIfFileExists);
        }

        public void ChangeLogFile(string path, BasicLoggerWriteMode actionIfFileExists)
        {
            StopLogging();
            BeginLogging(path, actionIfFileExists);
        }

        void BeginLogging(string path, BasicLoggerWriteMode actionIfFileExists)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new InvalidPathException("The path provided as an argument is either null, whitespace or empty.", path);
            if (!File.Exists(path))
                TryCreateFile(path);
            else
            {
                if (actionIfFileExists == BasicLoggerWriteMode.ThrowError)
                    throw new FileAlreadyExistsException("File already exists at " + path, path);
                else if (actionIfFileExists == BasicLoggerWriteMode.Overwrite)
                {
                    TryDeleteFile(path);
                    TryCreateFile(path);
                }
            }
            TryOpenFile(path);
            LogFilePath = path;

        }

        public LogMessageWLevel WriteToLog(string message, BasicLoggerLogLevel logLevel, bool includeDateTime = true)
        {
            DateTime dateTime = DateTime.Now;
            LogMessageWLevel retVal = new LogMessageWLevel(dateTime, logLevel, message, includeDateTime);
            TryWrite(retVal.ToString());
            return retVal;
        }
        public LogMessageWLevel WriteToLog(string message, DateTime dateTime, BasicLoggerLogLevel logLevel)
        {
            LogMessageWLevel retVal = new LogMessageWLevel(dateTime, logLevel, message);
            TryWrite(retVal.ToString());
            return retVal;
        }

        public LogMessage WriteToLog(string message, DateTime dateTime)
        {
            LogMessage retVal = new LogMessage(dateTime, message);
            TryWrite(retVal.ToString());
            return retVal;
        }

        public void WriteEmptyLineToLog()
        {
            TryWrite(null);
        }

        public LogMessage WriteToLog(string message, bool includeDateTime = true)
        {
            DateTime dateTime = DateTime.Now;
            LogMessage retVal = new LogMessage(dateTime, message, includeDateTime);
            TryWrite(retVal.ToString());
            return retVal;

        }

        public void StopLogging()
        {
            if (_openFile != null)
            {
                _openFile.Close();
                _openFile = null;
            }
        }


        internal void TryWrite(string message, bool asLine = true)
        {
            if (_openFile == null)
                throw new LogFilePathNotSetException("The path to the log file has not been set.");
            try
            {
                if (asLine)
                {
                    if (string.IsNullOrWhiteSpace(message))
                        _openFile.WriteLine();
                    else
                        _openFile.WriteLine(message);
                }
                else
                    _openFile.Write(message);
                _openFile.Flush();
            }
            catch
            {
                throw new UnableToWriteToFileException("Unable to write to log file " + LogFilePath, LogFilePath);
            }
        }

        protected void TryOpenFile(string path)
        {
            if (_openFile != null)
            {
                _openFile.Close();
                _openFile = null;
            }
            try
            {
                FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None);
                _openFile = new StreamWriter(fileStream);
            }
            catch
            {
                throw new UnableToOpenFileException("Unable to open file " + path + " for writing.", path);
            }
        }

        protected void TryDeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
                throw new UnableToDeleteFileException("Unable to delete file " + path, path);
            }
        }

        protected void TryCreateFile(string path)
        {
            try
            {
                FileStream fs = File.Create(path);
                fs.Close();
            }
            catch
            {
                throw new UnableToCreateFileException("Us unable to create file at " + path, path);
            }
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
