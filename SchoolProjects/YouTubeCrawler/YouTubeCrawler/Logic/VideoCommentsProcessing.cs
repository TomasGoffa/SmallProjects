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
    using System.Linq;
    using System.Reflection;

    using Google.Apis.Http;
    using Google.Apis.Services;
    using Google.Apis.YouTube.v3;
    using Google.Apis.YouTube.v3.Data;
    using YoutubeSearch;

    public class VideoCommentsProcessing : IDisposable
    {
        public YouTubeService MyYouTubeService { get; private set; }

        public VideoCommentsProcessing(string apiKey)
        {
            this.MyYouTubeService = new YouTubeService (new BaseClientService.Initializer ()
                    {
                        ApiKey = apiKey,
                        ApplicationName = Assembly.GetExecutingAssembly ().GetName ().Name
                    });
        }

        ~VideoCommentsProcessing()
        {
            this.Dispose ();
        }

        /// <summary>
        /// Starts the process of getting comments from Youtube videos
        /// </summary>
        /// <param name="filePath">file name where video tags are stored</param>
        /// <param name="numberOfPages">Number of pages on Youtube per tag. It defines number of videos per tag.</param>
        /// <returns></returns>
        public void StartProcessingComments(string filePath, int numberOfPages)
        {
            var videos = this.GetVideoUrls (filePath, numberOfPages);

            for (int i = 0; i < videos.Count; i++)
            {
                var videoId = this.GetVideoId (videos[i].Item2);
                var comments = this.GetCommentsFromVideo (videoId);
                this.SaveAttributesFromComments (comments, videos[i].Item1, videos[i].Item2, videos[i].Item3);                
            }
        }

        /// <summary>
        /// Finds all URLs for specified video tags.
        /// </summary>
        /// <param name="filePath">file name, where video tags are stored</param>
        /// <param name="numberOfPages">Number of pages on Youtube per tag. It defines number of videos per tag.</param>
        /// <returns>List of video URLs</returns>
        private List<(string, string, string)> GetVideoUrls (string filePath, int numberOfPages)
        {
            var videoTags = FileParserHelper.GetVideoTags (filePath);
            videoTags = videoTags.Where (x => !string.IsNullOrEmpty (x)).ToList ();

            var videoSearch = new VideoSearch ();
            var allUrls = new List<(string, string, string)> ();

            for (int i = 0; i < videoTags.Count; i++)
            {
                List<VideoInformation> videos = videoSearch.SearchQuery (videoTags[i], numberOfPages);

                for (int j = 0; j < videos.Count; j++)
                {
                    allUrls.Add ((videoTags[i], videos[j].Url, videos[i].Title));
                }
            }

            return allUrls;
        }

        /// <summary>
        /// Gets comments of video specified by URL
        /// </summary>
        /// <param name="videoId">Video ID</param>
        private IList<CommentThread> GetCommentsFromVideo (string videoId)
        {
            var commentThreadsRequest = this.MyYouTubeService.CommentThreads.List ("replies,snippet");
            commentThreadsRequest.VideoId = videoId;
            commentThreadsRequest.MaxResults = 100;
            try
            {
                var response = commentThreadsRequest.Execute ();
                return response.Items;
            }
            catch (Exception ex)
            {
                // Workeround: Adding comments to videos is sometimes disabled.
                if (!ex.Message.Contains("parameter has disabled comments"))
                {
                    throw ex;
                }
                else
                {
                    Logger.LogMessage (LogInfo.Error, ex.ToString ());
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the comment attributes
        /// </summary>
        /// <param name="comments">List of comments</param>
        /// <param name="videoTag">Video Tag</param>
        /// <param name="videoUrl">Video URL</param>
        private void SaveAttributesFromComments(IList<CommentThread> comments, string videoTag, string videoUrl, string videoTitle)
        {
            if (comments == null)
            {
                return;
            }

            for (int i = 0; i < comments.Count; i++)
            {
                Database.AddToDBS("dtbs.db",new DatabaseRow () {
                    AuthorName = comments[i].Snippet.TopLevelComment.Snippet.AuthorDisplayName,
                    WrittenComment = comments[i].Snippet.TopLevelComment.Snippet.TextOriginal,
                    LikeCount = comments[i].Snippet.TopLevelComment.Snippet.LikeCount,
                    ViewerRating = comments[i].Snippet.TopLevelComment.Snippet.ViewerRating,
                    PublishedAt = comments[i].Snippet.TopLevelComment.Snippet.PublishedAt,
                    VideoTag = videoTag,
                    VideoName = videoTitle,
                    VideoUrl = videoUrl});
            }
        }

        /// <summary>
        /// Gets the video ID which is part of video URL
        /// </summary>
        /// <param name="url">The video URL</param>
        /// <returns>The video ID</returns>
        private string GetVideoId(string url)
        {
            int indexFrom = url.IndexOf ("=");
            int indexTo = url.LastIndexOf ("&");

            if (indexFrom < indexTo)
            {
                return url.Substring (indexFrom + 1, indexTo - indexFrom);
            }
            else
            {
                return url.Substring (indexFrom + 1);
            }
        }

        public void Dispose ()
        {
            this.MyYouTubeService.Dispose ();
        }
    }
}