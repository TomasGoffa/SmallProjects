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
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;

    using Microsoft.Win32;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly MainWindowModel model;

        public MainWindowViewModel()
        {
            this.model = new MainWindowModel ();
            this.model.LearningDone += this.LearningDoneEventHandler;

            this.LoadImage = new SimpleCommand (this.LoadImageCommandHandler);
            this.LoadLastImage = new SimpleCommand (this.LoadLastImageCommandHandler);
            this.SaveImage = new SimpleCommand (this.SaveImageCommandHandler);
            this.StartLearningAlgorithm = new SimpleCommand (this.StartLearningAlgorithmCommandHandler);
            this.ExitApplication = new SimpleCommand (this.ExitApplicationCommandHandler);
        }

        public Bitmap LoadedImage { get; private set; }
        public Bitmap ResultImage { get; private set; }
        public BitmapSource ImageSource { get; private set; }

        public string Gamma { get; set; }
        public string Radius { get; set; }
        public string GridWidth { get; set; }
        public string GridHeight { get; set; }
        public string NumberOfIterations { get; set; }
        public bool IsLoadLastPictureEnabled => this.LoadedImage != null;
        public bool IsSaveImageEnabled => this.ResultImage != null;
        public bool IsStartAlgorithmEnabled => this.LoadedImage != null;

        public ICommand LoadImage { get; private set; }
        public ICommand LoadLastImage { get; private set; }
        public ICommand SaveImage { get; private set; }
        public ICommand StartLearningAlgorithm { get; private set; }      
        public ICommand ExitApplication { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Command Handlers

        private void LoadImageCommandHandler()
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog
                {
                    Filter = "bmp files (*.bmp)|*.bmp"
                };

                Nullable<bool> result = dlg.ShowDialog ();

                if (result == true)
                {
                    this.LoadedImage = new Bitmap (dlg.FileName);
                    this.ImageSource = Converters.ToBitmapSource (this.LoadedImage);
                    this.OnPropertyChanged (nameof (this.ImageSource));
                    this.RefreshButtons ();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.LogMessage (LogInfo.Error, ex.ToString ());
            }
        }

        private void LoadLastImageCommandHandler()
        {
            try
            {
                this.ImageSource = Converters.ToBitmapSource (this.LoadedImage);
                this.OnPropertyChanged (nameof (this.ImageSource));
                this.RefreshButtons ();
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.LogMessage (LogInfo.Error, ex.ToString ());
            }
        }

        private void SaveImageCommandHandler()
        {
            try
            {
                SaveOutputView view = new SaveOutputView
                {
                    DataContext = new SaveOutputViewModel (this.model)
                };

                view.Show ();
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.LogMessage (LogInfo.Error, ex.ToString ());
            }
        }

        private void StartLearningAlgorithmCommandHandler()
        {
            try
            {
                bool parsedGamma = double.TryParse (this.Gamma, NumberStyles.Any, CultureInfo.InvariantCulture, out double gamma);
                bool parsedRadius = double.TryParse (this.Radius, NumberStyles.Any, CultureInfo.InvariantCulture, out double radius);
                bool parsedWidth = int.TryParse (this.GridWidth, out int kohGridWidth);
                bool parsedHeight = int.TryParse (this.GridHeight, out int kohGridHeight);
                bool parsedIterations = int.TryParse (this.NumberOfIterations, out int numOfIterations);

                if (!parsedGamma || !parsedRadius || !parsedWidth ||
                    !parsedHeight || !parsedIterations)
                {
                    MessageBox.Show ("You set wrong values. Please set the correct values in Text boxes.");
                    return;
                }

                this.model.StartAlgorithm (this.LoadedImage, gamma, radius, kohGridWidth, kohGridHeight, numOfIterations);
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.LogMessage (LogInfo.Error, ex.ToString ());
            }
        }

        private void ExitApplicationCommandHandler()
        {
            Environment.Exit (0);
        }

        #endregion

        private void LearningDoneEventHandler(object sender, NewImageEventArgs e)
        {
            this.ResultImage = e.Image;
            this.ImageSource = Converters.ToBitmapSource (this.ResultImage);
            this.OnPropertyChanged (nameof (this.ImageSource));
            this.RefreshButtons ();
        }

        private void RefreshButtons()
        {
            this.OnPropertyChanged (nameof (this.IsSaveImageEnabled));
            this.OnPropertyChanged (nameof (this.IsLoadLastPictureEnabled));
            this.OnPropertyChanged (nameof (this.IsStartAlgorithmEnabled));
        }

        public void OnPropertyChanged (string name)
        {
            this.PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (name));
        }
    }
}
