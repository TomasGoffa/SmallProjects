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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class FileParserHelper
    {
        /// <summary>
        /// Parses all Video tags stored in text file
        /// </summary>
        /// <param name="filePath">Text file name</param>
        /// <returns>array of video tags</returns>
        public static List<string> GetVideoTags (string filePath)
        {
            if (!File.Exists (filePath))
            {
                Logger.LogMessage (LogInfo.Warning, "File does NOT exist: " + filePath);
                throw new ArgumentException ("File does NOT exist: " + filePath);
            }

            return File.ReadAllText (filePath).Split (new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList ();
        }

        /// <summary>
        /// Adds Video tag to text file.
        /// </summary>
        /// <param name="filePath">Text file name</param>
        /// <param name="tag">New tag</param>
        public static void AddVideoTag (string filePath, string tag)
        {
            if (!File.Exists (filePath))
            {
                Logger.LogMessage (LogInfo.Warning, "File does NOT exist: " + filePath);
                throw new ArgumentException ("File does NOT exist: " + filePath);
            }

            File.AppendAllText (filePath, Environment.NewLine + tag);
        }

        /// <summary>
        /// Removes video tag from text file.
        /// </summary>
        /// <param name="filePath">Text file name</param>
        /// <param name="tag">Tag to be removed</param>
        public static void DeleteVideoTag (string filePath, string tag)
        {
            if (!File.Exists (filePath))
            {
                Logger.LogMessage (LogInfo.Warning, "File does NOT exist: " + filePath);
                throw new ArgumentException ("File does NOT exist: " + filePath);
            }

            var lines = File.ReadAllLines (filePath).Where (line => line.Trim () != tag).ToArray ();
            File.WriteAllLines (filePath, lines);
        }

        /// <summary>
        /// Converts list to a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ListToString (List<string> input)
        {
            if (input.Count == 0)
                return string.Empty;

            string output = input[0];
            for (int i = 1; i < input.Count; i++)
            {
                output += '\n' + input[i];
            }
            return output;
        }
    }
}
