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
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for VideoCrawlerView.xaml
    /// </summary>
    public partial class VideoCrawlerView : Window
    {
        public ObservableCollection<string> VideoTags { get; private set; }

        public VideoCrawlerView()
        {
            InitializeComponent();

            try
            {
                if (!File.Exists(Constants.VideoTagsFilePath))
                {
                    using ( StreamWriter writer = new StreamWriter(Constants.VideoTagsFilePath) )
                    {
                        writer.Write(string.Empty);
                    }

                    MessageBox.Show("File with video tags has been removed or has never existed. There are no tags for now.",
                        "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                this.SetupListviewSource ();
                this.BtnDeleteTag.IsEnabled = false;
                this.TxtBoxNumOfVideos.Text = "1";
            }
            catch (Exception ex)
            {
                Logger.LogMessage (LogInfo.Error, ex.ToString());
                MessageBox.Show (ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Events

        private void MoveWindow (object sender, MouseButtonEventArgs e)
        {
            DragMove ();
        }

        private void CloseWindow (object sender, RoutedEventArgs e)
        {
            this.Close ();
        }

        private void NumberValidationTextBox (object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex ("[^0-9]+");
            e.Handled = regex.IsMatch (e.Text);
        }

        private void SelectedListViewItemChanged (object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.VideoTags.Contains((string)this.TagListView.SelectedItem))
            {
                this.BtnDeleteTag.IsEnabled = true;
            }
            else
            {
                this.BtnDeleteTag.IsEnabled = false;
            }
        }

        private void DeleteVideoTag (object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.VideoTags.Contains ((string)this.TagListView.SelectedItem))
                {
                    var selectedTag = (string)this.TagListView.SelectedItem;
                    this.VideoTags.Remove (selectedTag);
                    FileParserHelper.DeleteVideoTag (Constants.VideoTagsFilePath, selectedTag);
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage (LogInfo.Error, ex.ToString ());
                MessageBox.Show (ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddVideoTag (object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace (this.TxtBoxNewTag.Text))
                {
                    this.VideoTags.Add (this.TxtBoxNewTag.Text);
                    FileParserHelper.AddVideoTag (Constants.VideoTagsFilePath, this.TxtBoxNewTag.Text);
                    this.TxtBoxNewTag.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage (LogInfo.Error, ex.ToString ());
                MessageBox.Show (ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartCrawling (object sender, RoutedEventArgs e)
        {
            try
            {
                bool parsed = int.TryParse(this.TxtBoxNumOfVideos.Text, out int numberOfPages);

                if (!parsed)
                {
                    MessageBox.Show ("Please set valid number of videos.\nUse integers only.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.TxtBoxApiKey.Text))
                {
                    MessageBox.Show ("Please add Google API Key.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var videoCommentsProcessing = new VideoCommentsProcessing (this.TxtBoxApiKey.Text))
                {
                    videoCommentsProcessing.StartProcessingComments (Constants.VideoTagsFilePath, numberOfPages);
                }

                MessageBox.Show ("Crawling DONE.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.LogMessage (LogInfo.Error, ex.ToString ());
                MessageBox.Show (ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Methods

        private void SetupListviewSource()
        {
            var tags = FileParserHelper.GetVideoTags (Constants.VideoTagsFilePath);

            this.VideoTags = new ObservableCollection<string> ();

            foreach (var tag in tags)
            {
                if (!string.IsNullOrWhiteSpace(tag))
                {
                    this.VideoTags.Add (tag);
                }
            }

            this.TagListView.ItemsSource = this.VideoTags;
        }

        #endregion
    }
}
