using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Indieteur.BasicLoggingSystem
{
    public static class Extensions
    {
        public static BasicLoggingTools.LimitByEnum ToBasicLogLimit(this byte numeric)
        {
            switch (numeric)
            {
                case 1:
                    return BasicLoggingTools.LimitByEnum.Count;
                default:
                    return BasicLoggingTools.LimitByEnum.Days;
            }
        }
        

        public static string ToLogFormatFullName(this MethodBase method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            StringBuilder sb = new StringBuilder();
            bool hasBegun = false;
            if (parameters != null)
            {
                foreach (ParameterInfo param in parameters)
                {
                    if (!hasBegun)
                    {
                        sb.Append(param.ParameterType.FullName);
                        hasBegun = true;
                    }
                    else
                    {
                        sb.Append(", " + param.ParameterType.FullName);
                    }
                }
            }
            string methodFormattedName = method.MemberType.ToString() + "#" + method.Name;
            return method.DeclaringType.FullName + "." + methodFormattedName + "(" + sb.ToString() + ")";
            
        }

        public static string ToFileNameFormat(this DateTime dateTime, bool includeLogExtension = true)
        {
            if (includeLogExtension)
                return dateTime.ToString("yyyyMMdd-HHmmssfff", CultureInfo.InvariantCulture) + ".log";
            else
                return dateTime.ToString("yyyyMMdd-HHmmssfff", CultureInfo.InvariantCulture);
        }

        public static string ToLogStringFormat(this DateTime dateTime)
        {
            return dateTime.ToString("[yyyy-MM-dd HH:mm:ss:fff]", CultureInfo.InvariantCulture);
        }


        public static string ToLogStringFormat(this BasicLoggerLogLevel logLevel)
        {
            switch (logLevel)
            {
                case BasicLoggerLogLevel.CriticalError:
                    return "Critical Error";
                case BasicLoggerLogLevel.Error:
                    return "Error";
                case BasicLoggerLogLevel.Information:
                    return "Information";
                default:
                    return "Warning";
            }
        }

        public static BasicLoggerLogLevel ToBasicLoggerLogLevel(this BasicDebugLogger.DebugErrorType debugLogType)
        {
            switch (debugLogType)
            {
                case BasicDebugLogger.DebugErrorType.CriticalError:
                    return BasicLoggerLogLevel.CriticalError;
                default:
                    return BasicLoggerLogLevel.Error;
            }
        }

        public static BasicDebugLogger.DebugErrorType ToDebugErrorType(this BasicLoggerLogLevel errorType)
        {
            switch (errorType)
            {
                case BasicLoggerLogLevel.CriticalError:
                    return BasicDebugLogger.DebugErrorType.CriticalError;
                default:
                    return BasicDebugLogger.DebugErrorType.Error;
            }
        }

        //private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        //{
        //    return
        //      assembly.GetTypes()
        //              .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
        //              .ToArray();
        //}
    }
}
