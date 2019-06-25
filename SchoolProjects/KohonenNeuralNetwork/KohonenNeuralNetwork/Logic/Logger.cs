/// <summary>
/// This file is part of application
/// which implements Kohonen neural network.
/// 
/// Author:     Tomas Goffa
/// Created:    2018
/// </summary>

namespace KohonenNeuralNetwork
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;

    /// <summary>
    /// Specifies the type of logged information
    /// </summary>
    public enum LogInfo
    {
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// Class for logging the information.
    /// </summary>
    public static class Logger
    {
        private static string logFile = string.Empty;
        private static readonly string logPath = Directory.GetCurrentDirectory ();

        /// <summary>
        /// Creates Log file.
        /// </summary>
        public static void CreateLogFile ()
        {
            if (string.IsNullOrWhiteSpace (logFile))
            {
                logFile = GetLogFileName ();
            }

            string logFilePath = Path.Combine (logPath, logFile);

            if (!File.Exists (logFilePath))
            {
                using (StreamWriter writer = new StreamWriter (logFilePath))
                {
                    writer.Write (string.Empty);
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

            switch (info)
            {
                case LogInfo.Info:
                    content = DateTime.Now.ToLongTimeString () + "\t***** INFO *****" + Environment.NewLine + message;
                    break;

                case LogInfo.Warning:
                    content = DateTime.Now.ToLongTimeString () + "\t***** WARNING *****" + Environment.NewLine + message;
                    break;

                case LogInfo.Error:
                    content = DateTime.Now.ToLongTimeString () + "\t***** ERROR *****" + Environment.NewLine + message;
                    break;
            }

            if (!string.IsNullOrEmpty (content))
            {
                using (StreamWriter writer = new StreamWriter (logFilePath, true))
                {
                    writer.WriteLine (content);
                }
            }
        }

        /// <summary>
        /// Logs the position of cluster centers
        /// </summary>
        /// <param name="clusterCenters">List of cluster centers</param>
        /// <param name="filePath">Path to the folder</param>
        /// <param name="fileName">File name.</param>
        /// <returns>TRUE if info was written successfully, FALSE otherwise</returns>
        public static bool WriteResultToFile(List<(int, int)> clusterCenters, string filePath, string fileName)
        {
            if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            string fullFileName = Path.Combine (filePath, fileName);

            using (StreamWriter writer = new StreamWriter (fullFileName, true))
            {
                foreach (var centerPoint in clusterCenters)
                {
                    writer.WriteLine (centerPoint.Item1 + "\t" + centerPoint.Item2);
                }
            }

            return true;
        }

        /// <summary>
        /// Creates log file name.
        /// </summary>
        /// <returns>The file name</returns>
        private static string GetLogFileName ()
        {
            return "Log_" + DateTime.Now.Date.Year.ToString () +
                "_" + DateTime.Now.Date.Month.ToString () +
                "_" + DateTime.Now.Date.Day.ToString () + ".log";
        }
    }
}
