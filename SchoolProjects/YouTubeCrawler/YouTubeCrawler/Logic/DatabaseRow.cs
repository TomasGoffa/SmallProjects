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

    public class DatabaseRow
    {
        public int Id { get; set; }

        public string VideoTag { get; set; }

        public string AuthorName { get; set; }

        public long? LikeCount { get; set; }

        public string VideoName { get; set; }

        public DateTime? PublishedAt { get; set; }

        public string VideoUrl { get; set; }

        public string WrittenComment { get; set; }

        public string ViewerRating { get; set; }

        public override string ToString ()
        {
            return "VideoTag-->" + this.VideoTag 
                + "\tAuthorName-->" + this.AuthorName
                + "\tComment-->" + this.CheckForNewLines(this.WrittenComment)
                + "\tLikeCount-->" + this.LikeCount.ToString ()
                + "\tPublishedAt-->" + this.PublishedAt.ToString()
                + "\tVideoURL-->" + this.VideoUrl;
        }

        private string CheckForNewLines(string content)
        {
            while (content.Contains("\n"))
            {
                content.Replace ("\n", " ");
            }

            return content;
        }
    }
}
