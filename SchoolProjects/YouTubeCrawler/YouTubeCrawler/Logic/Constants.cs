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

    public enum LogInfo
    {
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// Class for storing constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Name of file where video tags are stored.
        /// </summary>
        public const string VideoTagsFileName = "VideoTags.txt";

        /// <summary>
        /// Full path of file where video tags are stored.
        /// </summary>
        public static readonly string VideoTagsFilePath = Path.Combine (Directory.GetCurrentDirectory (), VideoTagsFileName);
    }

    public class ListViewItem
    {
        public string Name { get; set; }

        public long? LikeCount { get; set; }
    }
}
