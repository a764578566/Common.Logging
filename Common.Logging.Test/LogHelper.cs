using System;
using System.Collections.Generic;
using System.Text;
using NLog.Targets;
using NLog.Common;
using Common.Logging;

namespace Test
{
    /// <summary>
    /// 日志管理对象
    /// </summary>
    public static class LogHelper
    {
        private static ILog s_log;
        private static string s_appname = null;

        public static void Init(string appName = null)
        {
            s_appname = appName;

            InternalLogger.LogToConsole = true;

            s_log = LogManager.GetLogger(appName ?? "App");
        }

        public static void BindingDiagnosticsDebugLog()
        {
            //System.Diagnostics.Debug.Listeners.Add(new Log4NetTracelogListener());
        }

        public static void SetupLogDatabaseConnectionString(string connectionString)
        {
            foreach (var target in NLog.LogManager.Configuration.AllTargets)
            {
                var dbTarget = target as DatabaseTarget;
                if (dbTarget == null)
                    continue;

                dbTarget.ConnectionString = connectionString;
            }
            NLog.LogManager.Configuration.Reload();

            Init();
        }

        public static void Trace(string message)
        {
            s_log.Trace(message);
        }
        public static void Trace(this object source, string message)
        {
            LogManager.GetLogger(source.GetType()).Trace(message);
        }


        public static void Debug(string message)
        {
            s_log.Debug(message);
        }
        public static void Debug(this object source, string message)
        {
            LogManager.GetLogger(source.GetType()).Debug(message);
        }

        public static void Info(string message)
        {
            s_log.Info(message);
        }
        public static void Info(this object source, string message)
        {
            LogManager.GetLogger(source.GetType()).Info(message);
        }

        public static void Error(string message, Exception ex = null)
        {
            s_log.Error(message, ex);
        }
        public static void Error(this object source, string message)
        {
            LogManager.GetLogger(source.GetType()).Error(message);
        }

        public static void Fatal(string message, Exception ex = null)
        {
            s_log.Fatal(message, ex);
        }
        public static void Fatal(this object source, string message)
        {
            LogManager.GetLogger(source.GetType()).Fatal(message);
        }
    }

    class Log4NetTracelogListener : System.Diagnostics.TraceListener
    {
        public override void Write(string message)
        {
            Console.WriteLine(message);
            LogHelper.Debug(message);
        }

        public override void WriteLine(string message)
        {
            Write(message);
        }

        public override void Write(string message, string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                Write(message);
                return;
            }

            if (category.Equals("trace", StringComparison.OrdinalIgnoreCase))
            {
                LogHelper.Trace(message);
                return;
            }

            if (category.Equals("info", StringComparison.OrdinalIgnoreCase))
            {
                LogHelper.Info(message);
                return;
            }

            if (category.Equals("error", StringComparison.OrdinalIgnoreCase))
            {
                LogHelper.Error(message);
                return;
            }

            if (category.Equals("fatal", StringComparison.OrdinalIgnoreCase))
            {
                LogHelper.Fatal(message);
                return;
            }
        }
    }
}
