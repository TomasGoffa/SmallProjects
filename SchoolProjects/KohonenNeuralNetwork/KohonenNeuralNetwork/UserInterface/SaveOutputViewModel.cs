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
    using System.IO;
    using System.Windows;
    using System.Windows.Input;

    using WinForms = System.Windows.Forms;

    public class SaveOutputViewModel : INotifyPropertyChanged
    {
        private readonly MainWindowModel model;

        public event PropertyChangedEventHandler PropertyChanged;

        public SaveOutputViewModel(MainWindowModel model)
        {
            this.model = model;
            this.model.OutputSaved += this.OutputSavedEventHandler;

            this.Browse = new SimpleCommand (this.BrowseCommandHandler);
            this.Save = new SimpleCommand (this.SaveCommandHandler);
            this.Exit = new Command<Window> (this.ExitCommandHandler);

            this.OutputDirectory = Directory.GetCurrentDirectory ();
            this.OutputFileName = "Result";
            this.IsSaveImage = true;
            this.IsSaveTextFile = true;

            this.RefreshView ();
        }

        public string OutputDirectory { get; set; }
        public string OutputFileName { get; set; }
        public bool IsSaveImage { get; set; }
        public bool IsSaveTextFile { get; set; }

        public ICommand Browse { get; private set; }
        public ICommand Save { get; private set; }
        public ICommand Exit { get; private set; }

        private void BrowseCommandHandler()
        {
            try
            {
                WinForms.FolderBrowserDialog fbd = new WinForms.FolderBrowserDialog ();

                if (fbd.ShowDialog () == WinForms.DialogResult.OK)
                {
                    this.OutputDirectory = fbd.SelectedPath;
                }

                this.RefreshView ();
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.LogMessage (LogInfo.Error, ex.ToString ());
            }
        }

        private void SaveCommandHandler()
        {
            try
            {
                if (!this.IsSaveImage && !this.IsSaveTextFile)
                {
                    MessageBox.Show ("You didn't choose any type of saving.");
                    return;
                }

                if (!Directory.Exists (this.OutputDirectory))
                {
                    MessageBox.Show ("Selected output directory doesn't exist. Please select existing directory.");
                    return;
                }

                this.model.SaveOutput (this.OutputDirectory, this.OutputFileName, this.IsSaveImage, this.IsSaveTextFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.LogMessage (LogInfo.Error, ex.ToString ());
            }
        }

        private void ExitCommandHandler(Window window)
        {
            window?.Close ();
        }

        private void OutputSavedEventHandler(object sender, EventArgs e)
        {
            MessageBox.Show ("Output has been saved.");
        }

        private void RefreshView()
        {
            this.OnPropertyChanged (nameof (this.OutputDirectory));
            this.OnPropertyChanged (nameof (this.OutputFileName));
            this.OnPropertyChanged (nameof (this.IsSaveImage));
            this.OnPropertyChanged (nameof (this.IsSaveTextFile));
        }

        public void OnPropertyChanged (string name)
        {
            this.PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (name));
        }
    }
}
