// YoutubeSearch
// YoutubeSearch is a library for .NET, written in C#, to show search query results from YouTube.
//
// (c) 2016 Torsten Klinger - torsten.klinger(at)googlemail.com
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YoutubeSearch
{
    public class VideoSearch
    {
        //constants for easier maintainability
        private const string Pattern = "<div class=\"yt-lockup-content\">.*?title=\"(?<NAME>.*?)\".*?</div></div></div></li>";
        private const string YtQueryUrl = "https://www.youtube.com/results?search_query=";
        private const string YtThumbnailUrl = "https://i.ytimg.com/vi/";
        private const string YtWatchUrl = "http://www.youtube.com/watch?v=";

        WebClient webclient;

        string viewCount;
        bool noDesc = false;
        bool noAuthor = false;

        /// <summary>
        /// Doing search query with given parameters. Returns a List<> object.
        /// </summary>
        /// <param name="querystring"></param>
        /// <param name="querypages"></param>
        /// <returns></returns>
        public async Task<List<VideoInformation>> SearchQueryTaskAsync (string querystring, int querypages)
        {
            var items = new List<VideoInformation> ();

            this.webclient = new WebClient ();

            // Do search
            for (int i = 1; i <= querypages; i++)
            {
                // Search address
                string html = await this.webclient.DownloadStringTaskAsync (YtQueryUrl + querystring + "&page=" + i);

                //extract information from page
                ProcessPage (html, items);
            }

            return items;
        }

        /// <summary>
        /// Doing search query with given parameters. Returns a List<> object.
        /// </summary>
        /// <param name="querystring"></param>
        /// <param name="querypages"></param>
        /// <returns></returns>
        public List<VideoInformation> SearchQuery (string querystring, int querypages)
        {
            var items = new List<VideoInformation> ();

            this.webclient = new WebClient ();

            // Do search
            for (int i = 1; i <= querypages; i++)
            {
                // Search address
                string html = this.webclient.DownloadString (YtQueryUrl + querystring + "&page=" + i);

                //extract information from page
                ProcessPage (html, items);
            }

            return items;
        }


        private void ProcessPage (string htmlPage, List<VideoInformation> items)
        {

            MatchCollection result = Regex.Matches (htmlPage, Pattern, RegexOptions.Singleline);

            for (int ctr = 0; ctr <= result.Count - 1; ctr++)
            {
                if (result[ctr].Value.Contains ("yt-uix-button-subscription-container\">") || result[ctr].Value.Contains ("\"instream\":true"))
                    continue; // Don't add to the list of search results if the value is a channel or live stream.

                // Title
                string title = result[ctr].Groups[1].Value;

                // Author
                string author = VideoItemHelper.Cull (result[ctr].Value, "/user/", "class").Replace ('"', ' ').TrimStart ().TrimEnd ();
                if (string.IsNullOrEmpty (author))
                {
                    author = VideoItemHelper.Cull (result[ctr].Value, " >", "</a>");
                    if (string.IsNullOrEmpty (author))
                        this.noAuthor = true;
                }

                // Description
                string description = VideoItemHelper.Cull (result[ctr].Value, "dir=\"ltr\" class=\"yt-uix-redirect-link\">", "</div>");
                if (string.IsNullOrEmpty (description))
                {
                    description = VideoItemHelper.Cull (result[ctr].Value, "<div class=\"yt-lockup-description yt-ui-ellipsis yt-ui-ellipsis-2\" dir=\"ltr\">", "</div>");
                    if (string.IsNullOrEmpty (description))
                        this.noDesc = true;
                }

                // Duration
                string duration = VideoItemHelper.Cull (VideoItemHelper.Cull (result[ctr].Value, "id=\"description-id-", "span"), ": ", "<").Replace (".", "");

                // Url
                string url = string.Concat (YtWatchUrl, VideoItemHelper.Cull (result[ctr].Value, "watch?v=", "\""));

                // Thumbnail
                string thumbnail = YtThumbnailUrl + VideoItemHelper.Cull (result[ctr].Value, "watch?v=", "\"") + "/mqdefault.jpg";

                // View Count
                {
                    string strView = VideoItemHelper.Cull (result[ctr].Value, "</li><li>", "</li></ul></div>");
                    if (!string.IsNullOrEmpty (strView) && !string.IsNullOrWhiteSpace (strView))
                    {
                        string[] strParsedArr = strView.Split (new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                        string parsedText = strParsedArr[0];
                        parsedText = parsedText.Trim ().Replace (",", ".");

                        this.viewCount = parsedText;
                    }
                }

                // Remove playlists
                if (title != "__title__" && duration != "")
                {
                    // Add item to list
                    items.Add (new VideoInformation () { Title = title, Author = author, Description = description, Duration = duration, Url = url, Thumbnail = thumbnail, NoAuthor = noAuthor, NoDescription = noDesc, ViewCount = this.viewCount });
                }

                // Reset values to default for next loop.
                this.noAuthor = false;
                this.noDesc = false;
            }
        }
    }
}
