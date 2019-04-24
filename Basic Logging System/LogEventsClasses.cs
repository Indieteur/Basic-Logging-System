using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Indieteur.BasicLoggingSystem
{
    public class LogMessage
    {
        public string Message { get; private set; }
        public bool HasDateAndTime { get; private set; }
        public DateTime TimeStamp { get; private set; }

        public LogMessage(DateTime timeStamp, string message = null, bool hasDateAndTime = true)
        {
            Message = message;
            HasDateAndTime = hasDateAndTime;
            TimeStamp = timeStamp;
        }

        public override string ToString()
        {
            if (HasDateAndTime)
            {
                StringBuilder sb = new StringBuilder(TimeStamp.ToLogStringFormat());
                if (!string.IsNullOrWhiteSpace(Message))
                    sb.Append(" " + Message);
                return sb.ToString();
            }
            else
                return Message;
        }
    }

    public class LogMessageWLevel : LogMessage
    {
        public BasicLoggerLogLevel LogLevel { get; private set; }
        public LogMessageWLevel(DateTime timeStamp, BasicLoggerLogLevel logLevel, string message = null, bool hasDateAndTime = true) : base(timeStamp, message, hasDateAndTime)
        {
            LogLevel = logLevel;
        }

        public override string ToString()
        {
            if (HasDateAndTime)
            {
                StringBuilder sb = new StringBuilder(TimeStamp.ToLogStringFormat() + " " + LogLevel.ToLogStringFormat());
                if (!string.IsNullOrWhiteSpace(Message))
                    sb.Append(": " + Message);
                return sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder(LogLevel.ToLogStringFormat());
                if (!string.IsNullOrWhiteSpace(Message))
                    sb.Append(": " + Message);
                return sb.ToString();
            }
        }
    }

    public class DebugLogMessageLevel : LogMessageWLevel
    {
        public string Namespace { get; private set; }
        public Exception @Exception { get; private set; }
        public string[] AdditionalMessages { get; private set; }
        public DebugLogMessageLevel(DateTime timeStamp, BasicLoggerLogLevel logLevel, string message = null, string _namespace = null, Exception exception = null, string[] additionalMessages = null, bool hasDateAndTime = true) : base(timeStamp, logLevel, message, hasDateAndTime)
        {
            Namespace = _namespace;
            Exception = exception;
            AdditionalMessages = additionalMessages;
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder((Exception != null) ? Environment.NewLine : string.Empty);
            if (HasDateAndTime)
                sb.Append(TimeStamp.ToLogStringFormat() + " " + LogLevel.ToLogStringFormat() + ": ");
            else
                sb.Append(LogLevel.ToLogStringFormat() + ": ");

            if (!string.IsNullOrWhiteSpace(Namespace))
                sb.Append(Namespace);
            Type exceptionType = null;
            if (Exception != null)
            {
                exceptionType = Exception.GetType();
                sb.Append(" | " + exceptionType.FullName);

            }
            if (!string.IsNullOrWhiteSpace(Message))
                sb.Append(" | " + Message);
        
            if (AdditionalMessages != null && AdditionalMessages.Length > 0)
            {
                foreach (string addMessage in AdditionalMessages)
                {
                    sb.Append(" | " + addMessage);
                }
            }

            if (Exception != null)
            {
                
                PropertyInfo[] propertyInfoArray = exceptionType.GetProperties();
                FieldInfo[] fieldInfoArray = exceptionType.GetFields();
                if (propertyInfoArray != null && propertyInfoArray.Length > 0)
                {
                    foreach (PropertyInfo pInfo in propertyInfoArray)
                    {
                        object value = pInfo.GetValue(Exception);
                        if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                            sb.Append(" | Exception " + pInfo.Name + ": " + value.ToString());
                    }

                }
                if (fieldInfoArray != null && fieldInfoArray.Length > 0)
                {
                    foreach (FieldInfo fInfo in fieldInfoArray)
                    {
                        object value = fInfo.GetValue(Exception);
                        if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                            sb.Append(" | Exception " + fInfo.Name + ": " + value.ToString());
                    }
                }
                sb.AppendLine();
            }



            return sb.ToString();
        }


    }
}
