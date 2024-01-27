using MWM.PrototypeTemplate;
using UnityEngine;


namespace VTemplate
{
    public enum LogLevel { Verbose, Warning, Error };
    public static class Logger
    {
        /// <summary>
        /// Logs a message in the console
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="logLevel">LogLevel to choose which Debug function is used (Log, LogWarning, LogError)</param>
        public static void Log (object message, LogLevel logLevel)
        {
            LoggerSettings settings = new LoggerSettings("", Color.white);
            Log(message, logLevel, settings);
        }

        /// <summary>
        /// Logs a message in the console
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="logLevel">LogLevel to choose which Debug function is used (Log, LogWarning, LogError)</param>
        /// <param name="settings">Settings of the log</param>
        public static void Log (object message, LogLevel logLevel, LoggerSettings settings)
        {
            Precondition.CheckNotNull(message);
            Precondition.CheckNotNull(logLevel);
            Precondition.CheckNotNull(settings);
            
            if (!ShouldDisplayLog(settings.Tag))
                return;

            switch (logLevel)
            {
                case LogLevel.Verbose:
                    Debug.Log(FormatLog(message.ToString(), settings.Tag, ColorUtility.ToHtmlStringRGBA(settings.Color)));
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(FormatWithTag(message.ToString(), settings.Tag));
                    break;
                case LogLevel.Error:
                    Debug.LogError(FormatWithTag(message.ToString(), settings.Tag));
                    break;
            }
        }

        private static bool ShouldDisplayLog (string tag)
        {
            Precondition.CheckNotNull(tag);
            if (LoggerPrefs.LogTagsToDisplay == null || LoggerPrefs.LogTagsToDisplay.Count == 0 || LoggerPrefs.LogTagsToDisplay.Contains(tag))
                return true;
            return false;
        }

        private static string FormatLog (string log, string tag, string color)
        {
            Precondition.CheckNotNull(log);
            Precondition.CheckNotNull(tag);
            Precondition.CheckNotNull(color);
            return FormatWithColor(FormatWithTag(log, tag), color);
        }

        private static string FormatWithTag (string log, string tag)
        {
            Precondition.CheckNotNull(log);
            Precondition.CheckNotNull(tag);
            
            string logTagged = $"[ACRAB]";
            if (tag != string.Empty)
                logTagged += $"[{tag}]";
            logTagged += $" {log}";
            return logTagged;
        }

        private static string FormatWithColor (string log, string color)
        {
            Precondition.CheckNotNull(log);
            Precondition.CheckNotNull(color);
            if (color == string.Empty)
                return log;
            return $"<color=#{color}>{log}</color>";
        }
    }
}