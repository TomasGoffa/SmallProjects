/// <summary>
/// This file is part of application
/// which implements Kohonen neural network.
/// 
/// Author:     Tomas Goffa
/// Created:    2018
/// </summary>

namespace KohonenNeuralNetwork
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView ()
        {
            InitializeComponent ();
            this.DataContext = new MainWindowViewModel ();
        }

        private void MoveWindow (object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove ();
        }
    }
}
