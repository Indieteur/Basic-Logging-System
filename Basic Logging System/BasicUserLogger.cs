using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indieteur.BasicLoggingSystem
{
    public class BasicUserLogger : BaseLogger
    {
        public BasicUserLogger(string path, BasicLoggerPathProvided typeOfPathProvided = BasicLoggerPathProvided.Directory, BasicLoggerWriteMode actionIfFileExists = BasicLoggerWriteMode.Overwrite) : base(path, typeOfPathProvided, actionIfFileExists)
        {
        }
    }
}
