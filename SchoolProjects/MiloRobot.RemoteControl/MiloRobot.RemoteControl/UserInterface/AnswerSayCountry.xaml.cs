/// <summary>
/// This file is part of project for remote
/// control of Robokind’s Milo Robot
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace MiloRobot.RemoteControl.UserInterface
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using MiloRobot.RemoteControl.Logic;

    /// <summary>
    /// Interaction logic for AnswerSayCountry.xaml
    /// </summary>
    public partial class AnswerSayCountry : Window
    {
        /// <summary>
        /// Instance of MainWindow.
        /// </summary>
        private MainWindow myMainWindow;

        public AnswerSayCountry (MainWindow mainWindow)
        {
            InitializeComponent ();

            this.myMainWindow = mainWindow;
        }

        private void WindowMouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        {
            DragMove ();
        }

        private void SendBtn_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                this.myMainWindow.SendAnswerToSayCountry (AnswerTextBox.Text);
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("AnswerSayCountry: \n" + ex.ToString ());
            }
        }

        private void CloseBtn_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close ();
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("AnswerSayCountry: \n" + ex.ToString ());
            }
        }
    }
}
