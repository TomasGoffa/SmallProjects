/// <summary>
/// This file is part of project for remote
/// control of Robokind’s Milo Robot
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace MiloRobot.RemoteControl.Logic
{
    using System;
    using System.Globalization;
    using System.IO;

    public static class Logger
    {
        public static void LogMessage(string message)
        {
            string dateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            string timeFormat = CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;
            string path = Path.Combine (Directory.GetCurrentDirectory(), GetLogFileName());

            using (StreamWriter writer = new StreamWriter (path, true))
            {
                writer.WriteLine (DateTime.Now.ToString (dateFormat + "  " + timeFormat) + ": " + message);
            }
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
