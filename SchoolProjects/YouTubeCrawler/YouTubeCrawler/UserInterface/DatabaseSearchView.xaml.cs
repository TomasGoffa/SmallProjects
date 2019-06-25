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
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for DatabaseSearchView.xaml
    /// </summary>
    public partial class DatabaseSearchView : Window
    {
        private const string databaseRoute = "dtbs.db";
        private const string selectAll = "Select All";
        private const string videoTag = "VideoTag";
        private const string authorName = "AuthorName";
        private const string likeCount = "LikeCount";
        private const string videoName = "VideoName";
        private const string publishedAt = "PublishedAt";
        private const string videoUrl = "VideoUrl";
        private const string writtenComment = "WrittenComment";
        private const string viewerRating = "ViewerRating";

        public DatabaseSearchView ()
        {
            InitializeComponent ();

            Logger.CreateLogFile ();

            this.CmbFilterOption.Items.Add (selectAll);
            this.CmbFilterOption.Items.Add (videoTag);
            this.CmbFilterOption.Items.Add (authorName);
            this.CmbFilterOption.Items.Add (likeCount);
            this.CmbFilterOption.Items.Add (videoName);
            this.CmbFilterOption.Items.Add (publishedAt);
            this.CmbFilterOption.Items.Add (videoUrl);
            this.CmbFilterOption.Items.Add (writtenComment);
            this.CmbFilterOption.Items.Add (viewerRating);
            this.CmbFilterOption.SelectedItem = selectAll;

            this.DatabaseDatagrid.ItemsSource = this.GetDatabase ();
        }

        #region Event Handlers

        private void MoveWindow (object sender, MouseButtonEventArgs e)
        {
            DragMove ();
        }

        private void ExitApplication (object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown ();
        }

        private void OpenCrawler (object sender, RoutedEventArgs e)
        {
            var videoCrawlerView = new VideoCrawlerView ();
            videoCrawlerView.Show ();
        }

        private void BtnFilter_Click (object sender, RoutedEventArgs e)
        {
            string filterOpt = (string)this.CmbFilterOption.SelectedItem;
            string filter = this.TextInputField.Text;

            if (filterOpt.Equals(selectAll))
            {
                this.DatabaseDatagrid.ItemsSource = null;
                this.DatabaseDatagrid.ItemsSource = this.GetDatabase ();
                return;
            }

            if (string.IsNullOrEmpty(filter))
            {
                MessageBox.Show ("Please enter filter tag", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            this.DatabaseDatagrid.ItemsSource = null;

            switch (filterOpt)
            {
                case videoTag:
                    this.DatabaseDatagrid.ItemsSource = this.GetDatabase ().Where (x => x.VideoTag == TextInputField.Text).ToList ();
                    break;
                case authorName:
                    this.DatabaseDatagrid.ItemsSource = this.GetDatabase ().Where (x => x.AuthorName == TextInputField.Text).ToList ();
                    break;
                case likeCount:
                    this.DatabaseDatagrid.ItemsSource = this.GetDatabase ().Where (x => x.LikeCount.ToString() == TextInputField.Text).ToList ();
                    break;
                case videoName:
                    this.DatabaseDatagrid.ItemsSource = this.GetDatabase ().Where (x => x.VideoName == TextInputField.Text).ToList ();
                    break;
                case publishedAt:
                    this.DatabaseDatagrid.ItemsSource = this.GetDatabase ().Where (x => x.PublishedAt.ToString() == TextInputField.Text).ToList ();
                    break;
                case videoUrl:
                    this.DatabaseDatagrid.ItemsSource = this.GetDatabase ().Where (x => x.VideoUrl == TextInputField.Text).ToList ();
                    break;
                case writtenComment:
                    this.DatabaseDatagrid.ItemsSource = this.GetDatabase ().Where (x => x.WrittenComment == TextInputField.Text).ToList ();
                    break;
                case viewerRating:
                    this.DatabaseDatagrid.ItemsSource = this.GetDatabase ().Where (x => x.ViewerRating == TextInputField.Text).ToList ();
                    break;
                default:
                    this.DatabaseDatagrid.ItemsSource = this.GetDatabase ();
                    break;
            }
        }

        private void ClearDBS_Click (object sender, RoutedEventArgs e)
        {
            Database.ClearDBS (databaseRoute);
            this.DatabaseDatagrid.ItemsSource = null;
            this.DatabaseDatagrid.ItemsSource = this.GetDatabase ();
        }

        #endregion

        #region Methods

        private List<DatabaseRow> GetDatabase ()
        {
            List<DatabaseRow> toReturn = new List<DatabaseRow> ();

            try
            {
                foreach (var row in Database.SelectAllDatabase (databaseRoute))
                {
                    toReturn.Add (row);
                }

                if (toReturn.Count == 0)
                {
                    MessageBox.Show ("Database is empty", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                return toReturn;
            }
            catch (Exception ex)
            {
                MessageBox.Show ("Database is empty", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                Logger.LogMessage (LogInfo.Error, ex.ToString());
                return new List<DatabaseRow> ();
            }
        }

        #endregion
    }
}
