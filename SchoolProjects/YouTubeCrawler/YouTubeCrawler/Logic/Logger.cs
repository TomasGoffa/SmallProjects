/// <summary>
/// This file is part of school project for
/// Semantic and Social Web
/// 
/// Authors     : Matej Kvetko
///             : Tomas Goffa
/// </summary>

namespace YouTubeCrawler
{
    using System;
    using System.IO;

    /// <summary>
    /// Class for logging information.
    /// </summary>
    public static class Logger
    {
        private static string logFile = string.Empty;
        private static readonly string logPath = Directory.GetCurrentDirectory ();

        /// <summary>
        /// Creates Log file.
        /// </summary>
        public static void CreateLogFile()
        {
            if (string.IsNullOrWhiteSpace (logFile))
            {
                logFile = GetLogFileName ();
            }

            string logFilePath = Path.Combine (logPath, logFile);

            if (!File.Exists (logFilePath))
            {
                using ( StreamWriter writer = new StreamWriter(logFilePath) )
                {
                    writer.Write(string.Empty);
                }
            }
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        public static void LogMessage (LogInfo info, string message)
        {
            CreateLogFile ();

            string logFilePath = Path.Combine (logPath, logFile);
            string content = string.Empty;

            switch(info)
            {
                case LogInfo.Info:
                    content = DateTime.Now.ToLongTimeString () + " : INFO    : " + message;
                    break;

                case LogInfo.Warning:
                    content = DateTime.Now.ToLongTimeString () + " : WARNING : " + message;
                    break;

                case LogInfo.Error:
                    content = DateTime.Now.ToLongTimeString () + " : ERROR   : " + message;
                    break;
            }

            if (!string.IsNullOrEmpty(content))
            {
                using ( StreamWriter writer = new StreamWriter(logFilePath, true) ) {
                    writer.WriteLine(content);
                }
            }
        }

        /// <summary>
        /// Creates log file name.
        /// </summary>
        /// <returns>The file name</returns>
        private static string GetLogFileName()
        {
            return "Log_" + DateTime.Now.Date.Year.ToString () +
                "_" + DateTime.Now.Date.Month.ToString () +
                "_" + DateTime.Now.Date.Day.ToString () + ".log";
        }
    }
}
