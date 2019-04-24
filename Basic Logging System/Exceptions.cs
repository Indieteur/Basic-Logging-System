using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indieteur.BasicLoggingSystem
{

    public class NoDebugLogException : Exception
    {
        public NoDebugLogException(string messgae) : base(messgae)
        {

        }
    }

    public class NoUserLogException: Exception
    {
        public NoUserLogException(string messgae) : base(messgae)
        {

        }
    }

    public class MultipleExceptions : Exception
    {
        public List<Exception> Exceptions { get; protected set; } = new List<Exception>();
        public MultipleExceptions(string message, params Exception[] exceptions) : base(message)
        {
            if (exceptions != null)
                Exceptions.AddRange(exceptions);
        }
    }
    public class LogFilePathNotSetException : Exception
    {
        public LogFilePathNotSetException(string message) : base(message)
        {

        }
    }


    public class InvalidPathException : FileSystemException
    {
        public InvalidPathException(string path) : base (path)
        {

        }
        public InvalidPathException(string message, string path) : base(message, path)
        {
        }
    }

    public class DirectoryExistsException : FileSystemException
    {
        public DirectoryExistsException(string path) : base(path)
        {

        }
        public DirectoryExistsException(string message, string path) : base(message, path)
        {
        }
    }

    public class UnableToWriteToFileException : FileSystemException
    {
        public UnableToWriteToFileException(string message, string path) : base(message, path)
        {
        }
    }

    public class UnableToOpenFileException : FileSystemException
    {
        public UnableToOpenFileException(string message, string path) : base(message, path)
        {
        }
    }

    public class UnableToCreateFileException : FileSystemException
    {
        public UnableToCreateFileException(string message, string path) : base(message, path)
        {
        }
    }

    public class UnableToDeleteFileException : FileSystemException
    {
        public UnableToDeleteFileException(string message, string path) : base(message, path)
        {
        }
    }

    public class FileAlreadyExistsException : FileSystemException
    {
        public FileAlreadyExistsException(string message, string path) : base(message, path)
        {
        }
    }
    

    public abstract class FileSystemException : Exception
    {
        public string Path { get; protected set; }
        public FileSystemException(string message, string path) : base(message)
        {
            Path = path;
        }
        public FileSystemException(string path)
        {
            Path = path;
        }
    }

   
}
